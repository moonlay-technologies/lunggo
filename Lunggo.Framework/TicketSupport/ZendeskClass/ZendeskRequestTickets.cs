using System.Threading.Tasks;

namespace Lunggo.Framework.TicketSupport.ZendeskClass
{
    public interface ITickets : ICore
    {
		ZendeskIndividualTicketResponse CreateTicket(ZendeskTicket ticket);
		ZendeskIndividualTicketResponse UpdateTicket(ZendeskTicket ticket, ZendeskComment comment=null);
		Task<ZendeskIndividualTicketResponse> CreateTicketAsync(ZendeskTicket ticket);
		Task<ZendeskIndividualTicketResponse> UpdateTicketAsync(ZendeskTicket ticket, ZendeskComment comment=null);
    }

    public class Tickets : ZendeskCore, ITickets
    {
        private const string _tickets = "tickets";
        private const string _ticket_forms = "ticket_forms";
        private const string _views = "views";
        private const string _organizations = "organizations";
        private const string _ticket_metrics = "ticket_metrics";


        public Tickets(string yourZendeskUrl, string user, string password, string apiToken)
            : base(yourZendeskUrl, user, password, apiToken)
        {
        }

        public ZendeskIndividualTicketResponse CreateTicket(ZendeskTicket ticket)
        {
            var body = new {ticket};
            return GenericPost<ZendeskIndividualTicketResponse>(_tickets + ".json", body);
        }

        public ZendeskIndividualTicketResponse UpdateTicket(ZendeskTicket ticket, ZendeskComment comment=null)
        {
            if (comment != null)
                ticket.Comment = comment;
            var body = new { ticket };

            return GenericPut<ZendeskIndividualTicketResponse>(string.Format("{0}/{1}.json", _tickets, ticket.Id), body);    
        }



        

        public async Task<ZendeskIndividualTicketResponse> CreateTicketAsync(ZendeskTicket ticket)
        {
            var body = new {ticket};
            return await GenericPostAsync<ZendeskIndividualTicketResponse>(_tickets + ".json", body);
        }

        /// <summary>
        /// UpdateTicket a ticket or add comments to it. Keep in mind that somethings like the description can't be updated.
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public async Task<ZendeskIndividualTicketResponse> UpdateTicketAsync(ZendeskTicket ticket, ZendeskComment comment=null)
        {
            if (comment != null)
                ticket.Comment = comment;
            var body = new { ticket };

            return await GenericPutAsync<ZendeskIndividualTicketResponse>(string.Format("{0}/{1}.json", _tickets, ticket.Id), body);    
        }


    }
}
