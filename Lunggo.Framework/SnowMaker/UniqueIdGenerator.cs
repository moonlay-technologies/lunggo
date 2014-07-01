using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace Lunggo.Framework.SnowMaker
{
    public class UniqueIdGenerator : IUniqueIdGenerator
    {
        private static readonly UniqueIdGenerator Instance = new UniqueIdGenerator();
        
        private IOptimisticDataStore _optimisticDataStore;
        private readonly IDictionary<string, ScopeState> _states = new Dictionary<string, ScopeState>();
        private readonly object _statesLock = new object();
        private readonly IDictionary<String, long> _initialValueDictionary = new Dictionary<String, long>(); 
        private int _batchSize = 100;
        private int _maxWriteAttempts = 25;

        private UniqueIdGenerator()
        {
            ;
        }

        public static UniqueIdGenerator GetInstance()
        {
            return Instance;
        }

        public void Init (IOptimisticDataStore optimisticDataStore)
        {
            lock (this)
            {
                if (_optimisticDataStore == null)
                {
                    this._optimisticDataStore = optimisticDataStore;
                }
                else
                {
                    throw new InvalidOperationException("Data Store is already initialized");
                }    
            }
        }

        public void SetIdInitialValue(String name, long value)
        {
            _initialValueDictionary[name] = value;
        }

        public long GetIdInitialValue(String name)
        {
            return _initialValueDictionary[name];
        }

        public int BatchSize
        {
            get { return _batchSize; }
            set { _batchSize = value; }
        }

        public int MaxWriteAttempts
        {
            get { return _maxWriteAttempts; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value", _maxWriteAttempts, "MaxWriteAttempts must be a positive number.");

                _maxWriteAttempts = value;
            }
        }

        public long NextId(string scopeName)
        {
            var state = GetScopeState(scopeName);

            lock (state.IdGenerationLock)
            {
                if (state.LastId == state.HighestIdAvailableInBatch)
                    UpdateFromSyncStore(scopeName, state);

                return Interlocked.Increment(ref state.LastId);
            }
        }

        ScopeState GetScopeState(string scopeName)
        {
            return _states.GetValue(
                scopeName,
                _statesLock,
                () => new ScopeState());
        }

        void UpdateFromSyncStore(string scopeName, ScopeState state)
        {
            var writesAttempted = 0;

            while (writesAttempted < _maxWriteAttempts)
            {
                var data = _optimisticDataStore.GetData(scopeName);

                long nextId;
                if (!long.TryParse(data, out nextId))
                    throw new UniqueIdGenerationException(string.Format(
                       "The id seed returned from storage for scope '{0}' was corrupt, and could not be parsed as a long. The data returned was: {1}",
                       scopeName,
                       data));

                state.LastId = nextId - 1;
                state.HighestIdAvailableInBatch = nextId - 1 + _batchSize;
                var firstIdInNextBatch = state.HighestIdAvailableInBatch + 1;

                if (_optimisticDataStore.TryOptimisticWrite(scopeName, firstIdInNextBatch.ToString(CultureInfo.InvariantCulture)))
                    return;

                writesAttempted++;
            }

            throw new UniqueIdGenerationException(string.Format(
                "Failed to update the data store after {0} attempts. This likely represents too much contention against the store. Increase the batch size to a value more appropriate to your generation load.",
                writesAttempted));
        }
    }
}
