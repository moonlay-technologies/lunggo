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
                            <input name="ccNo" type="number" className="form-control form-payment validation-form" id="nokartu" placeholder="xxx xxx xxx" onChange={props.handleInputChange} />
                            <div className="text-validation">{props.errorMessages.ccNo}</div>
                        </div>
                        <div className="form-group">
                            <label className="label-form" htmlFor="namakartu">Nama di Kartu</label>
                            <input name="name" type="text" className="form-control form-payment validation-form" id="namakartu" placeholder="John Doe" onChange={props.handleInputChange} />
                            <div className="text-validation">{props.errorMessages.name}</div>
                        </div>

                        {props.method === 'card' &&
                            <div className="row">
                                <div className="col-xs-6 no-padding-left">
                                    <div className="form-group no-margin">
                                        <label className="label-form" htmlFor="expiry">Berlaku Hingga</label>
                                        <input name="expiry" type="number" className="form-control form-payment" id="expiry" placeholder="08/20" onChange={props.handleInputChange} />
                                        <div className="text-validation">{props.errorMessages.expiry}</div>
                                    </div>
                                </div>
                                <div className="col-xs-6 no-padding-right">
                                    <div className="form-group no-margin">
                                        <label className="label-form" htmlFor="cvv">CVV 3 Digit</label>
                                        <input name="cvv" type="number" className="form-control form-payment" id="cvv" placeholder="xxx" onChange={props.handleInputChange} />
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
export default PaymentDataForm;