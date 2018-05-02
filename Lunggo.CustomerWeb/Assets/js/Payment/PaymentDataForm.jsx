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
                            <input type="number" className="form-control form-payment validation-form" id="nokartu" placeholder="xxx xxx xxx" value={props.ccNo} />
                            <div className="text-validation">{props.errorMessage.ccNo}</div>
                        </div>
                        <div className="form-group">
                            <label className="label-form" htmlFor="namakartu">Nama di Kartu</label>
                            <input type="text" className="form-control form-payment validation-form" id="namakartu" placeholder="John Doe" value={props.name} />
                            <div className="text-validation">{props.errorMessage.name}</div>
                        </div>

                        {props.method === 'creditCard' &&
                            <div className="row">
                                <div className="col-xs-6 no-padding-left">
                                    <div className="form-group no-margin">
                                        <label className="label-form" htmlFor="expiry-date">Berlaku Hingga</label>
                                        <input type="number" className="form-control form-payment" id="expiry-date" placeholder="08/20" value={props.expiry} />
                                        <div className="text-validation">{props.errorMessage.expiry}</div>
                                    </div>
                                </div>
                                <div className="col-xs-6 no-padding-right">
                                    <div className="form-group no-margin">
                                        <label className="label-form" htmlFor="cvv">CVV 3 Digit</label>
                                        <input type="number" className="form-control form-payment" id="cvv" placeholder="xxx" value={props.cvv} />
                                        <div className="text-validation">{props.errorMessage.cvv}</div>
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
export default PaymentDataForm;