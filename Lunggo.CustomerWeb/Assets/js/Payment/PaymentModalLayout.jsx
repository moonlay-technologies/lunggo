'use strict';
import React from 'react';
import DataForm from './PaymentDataForm.jsx';
import PaymentInstruction from './PaymentInstruction.jsx';
//import Iframe from 'react-iframe';

function PaymentModalLayout(props) {
  console.log(props.iframeUrl)
    return (
      <div id="payment-modal" className="mother-container modal fade">
        <div className="modal-dialog modal-lg" style={{
          background: 'white',
          padding: 10,
          borderRadius: 7
        }}>
          <div>
            {/*<Iframe
            //  hidden={props.paymentStep !== 'paymentOtp'}
            //  //url={props.iframeUrl}
            //  url="https://www.travorama.com"
            //  width="400px"
            //  height="420px"
            //  id="myId"
            //  className="myClassname"
            //  display="initial"
            //  position="relative"
            //  allowFullScreen />
            */}
            <iframe hidden={props.paymentStep !== 'paymentOtp'}
              src={props.iframeUrl}></iframe>
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
                            <a onClick={props.onSubmit} className="button-primary">Bayar Sekarang</a>
                        </div>
                        <div className="col-xs-12 no-padding">
                            <a data-dismiss="modal" className="button-secondary">Ganti Metode Pembayaran</a>
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