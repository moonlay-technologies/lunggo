'use strict';
import React from 'react';
import DataForm from './PaymentDataForm.jsx';
import PaymentInstruction from './PaymentInstruction.jsx';
//import Iframe from 'react-iframe';

function PaymentModalLayout(props) {
    return (
      <div id="payment-modal" className="mother-container modal fade">
        <div className="modal-dialog modal-lg" style={{
          background: 'white',
          padding: 10,
          borderRadius: 7
        }}>
          <div hidden={props.paymentStep !== 'loading'}>
            ...Loading
          </div>
          <div hidden={props.paymentStep !== 'failed'}>
            <div className="mother-container">
              <div style={{ textAlign: 'center', marginTop: '40%' }}>
                <div className="icon-success">
                  <img style={{width:100}} src="images/icon-error.png" />
                </div>
                <div className="text-success">{props.paymentStepStringData}</div>
                <div className="button-container-thankyou">
                  <div className="row">
                    <div className="col-xs-12 no-padding">
                      <a href="#" data-dismiss="modal" className="button-primary">Gunakan Metode Pembayaran Lain</a>
                    </div>
                  </div>
                </div>
              </div>

            </div>
          </div>
          <div hidden={props.paymentStep !== 'success'}>
              <div className="mother-container">
              <div style={{ textAlign: 'center', marginTop: '40%' }}>
                  <div className="icon-success">
                  <img style={{width:100}} src="images/icon-success.png" />
        </div>
                    <div className="text-success">Proses Pembayaran Berhasil</div>
                    <div className="text-success">No. Transaksi: 1234567890</div>
                    <div className="button-container-thankyou">
                      <div className="row">
                        <div className="col-xs-12 no-padding">
                          <a href="#" className="button-primary">Kembali ke Pesananku</a>
                        </div>
                      </div>
                    </div>
                  </div>

                </div> 
          </div>
          <div>
            <iframe hidden={props.paymentStep !== 'paymentOtp'} src={props.paymentStepStringData} width="400px" height="420px">
              <p>Your browser doesn't support iframe</p>
            </iframe>
          </div>
          <div hidden={props.paymentStep !== 'initial'}>
            <div className="row">
                <div className="col-xs-8 no-padding-left">
                  <div className="info-total-harga">
                    <p>Total Pembayaran</p>
                        <span>{props.refund}</span>
                    </div>
                </div>
                <div className="col-xs-4 text-right no-padding-right no-padding-left">
                    <div className="info-total-angka">
                        <p>{props.originalPrice}</p>
                    </div>
                </div>
            </div>

            {props.shouldShowDataForm && <DataForm {...props} />}
            <PaymentInstruction />


            <div className="section-container">
                <div className="more-info">
                    Dengan klik tombol bayar, anda telah setuju dengan <a href={props.termsUrl}>Syarat & Ketentuan</a> dan <a href={props.privacyUrl}>Kebijakan</a> yang berlaku
                </div>
                <div className="button-container">
                    <div className="row">
                        <div className="col-xs-12 no-padding">
                            <a href="#" onClick={props.onSubmit} className="button-primary">Bayar Sekarang</a>
                        </div>
                        <div className="col-xs-12 no-padding">
                            <a href="#" data-dismiss="modal" className="button-secondary">Ganti Metode Pembayaran</a>
                        </div>
                    </div>
                </div>
            </div>

          </div>
        </div>
      </div>
    );
}
export default PaymentModalLayout;