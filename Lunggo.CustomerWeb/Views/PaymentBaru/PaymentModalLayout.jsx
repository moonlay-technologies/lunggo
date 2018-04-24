'use strict';
import React from 'react';
import DataForm from './PaymentDataStateContainer';
import PaymentInstruction from './PaymentInstruction';

export default function PaymentModalLayout(props) {
  const shouldShowDataForm = (props.method == 'creditCard' || props.method == 'mandiriClickPay');
  return(
    <div hidden={props.method == null} className="mother-container">

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

      { if (shouldShowDataForm) {<DataForm ref={props.bindFormRef} {props} />} }
      <PaymentInstruction />


      <div className="section-container">
        <div className="more-info">
          Dengan klik tombol bayar, anda telah setuju dengan <a href={props.termsUrl}>Syarat & Ketentuan</a> dan <a href={props.privacyUrl}>Kebijakan</a> yang berlaku
        </div>
        <div className="button-container">
          <div className="row">
            <div className="col-xs-12 no-padding">
              <a onClick={props.pay} href="#" className="button-primary">Bayar Sekarang</a>
            </div>
            <div className="col-xs-12 no-padding">
              <a href="#" className="button-secondary">Ganti Metode Pembayaran</a>
            </div>
          </div>
        </div>
      </div>

    </div>
  );
}