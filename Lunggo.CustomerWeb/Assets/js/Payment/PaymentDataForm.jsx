'use strict';
import React from 'react';

function PaymentDataForm(props) {
  return (
    <div className="section-container">
      <div className="section-label">Personal Information</div>
      <div className="row">
        <div className="col-xs-12 info-container">
          <form>
            <div className="form-group">
              <label className="label-form" htmlFor="nokartu">No. Kartu</label>
              <input name="ccNo" type="number" id="nokartu" placeholder="xxx xxx xxx" className={props.errorMessages.ccNo ? "form-control form-payment validation-form" : "form-control form-payment"} onChange={props.handleInputChange} />
              <div className="text-validation">{props.errorMessages.ccNo}</div>
            </div>
            <div className="form-group">
              <label className="label-form" htmlFor="name">Nama di Kartu</label>
              <input name="name" type="text" id="namakartu" placeholder="John Doe" className={props.errorMessages.name ? "form-control form-payment validation-form" : "form-control form-payment"} onChange={props.handleInputChange} />
              <div className="text-validation">{props.errorMessages.name}</div>
            </div>

            {props.method === 'card' &&
              <div className="row">
              <div className="col-xs-6 row no-padding-left">
                <div className="row">
                  <label className="label-form" htmlFor="expiryMonth">Berlaku Hingga</label>
                </div>
                  <div className="col-xs-6 no-padding-left no-padding-right">
                    <div className="form-group no-margin">
                      <input name="expiryMonth" placeholder="MM" type="number" className={props.errorMessages.expiry ? "form-control form-payment validation-form" : "form-control form-payment"} onChange={e => props.handleInputChange(e, 'expiry')} />
                      <div className="text-validation">{props.errorMessages.expiry}</div>
                    </div>
                  </div>
                  <div className="col-xs-6 no-padding-left no-padding-right">
                    <div className="form-group no-margin">
                      <input name="expiryYear" placeholder="YY" type="number" className={props.errorMessages.expiry ? "form-control form-payment validation-form" : "form-control form-payment"} onChange={e => props.handleInputChange(e, 'expiry')} />
                    </div>
                  </div>
                </div>
                <div className="col-xs-6 no-padding-right">
                  <div className="form-group no-margin">
                  <label className="label-form" htmlFor="cvv">CVV 3 Digit</label>
                  <input name="cvv" type="number" className={props.errorMessages.cvv ? "form-control form-payment validation-form" : "form-control form-payment"} id="cvv" placeholder="xxx" onChange={props.handleInputChange} onKeyPress={e => event.keyCode === 13 && props.onSubmit() } />
                    <div className="text-validation">{props.errorMessages.cvv}</div>
                  </div>
                </div>
              </div>
            }
          </form>
        </div>
      </div>
    </div>
  );
}

//function ControlledInput(props) {
//  const errorName = (props.name == 'expiryMonth' || props.name == 'expiryYear') ? 'expiry' : props.name;
//  return <input
//    className={props.errorMessages[errorName] ? "form-control form-payment validation-form" : "form-control form-payment"}
//    onChange={ props.handleInputChange}
//    //onChange={(props.name == 'expiryMonth' || props.name == 'expiryYear') ? e => props.handleInputChange(e,'expiry') : props.handleInputChange}
//    {...props}
//  />
//}

export default PaymentDataForm;