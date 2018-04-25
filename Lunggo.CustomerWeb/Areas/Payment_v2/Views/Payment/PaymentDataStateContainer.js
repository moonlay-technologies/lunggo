'use strict';
import React from 'react';
import { observable, action } from "mobx";
import { observer } from "mobx-react";
import PaymentDataForm from './PaymentDataForm';

export default const PaymentDataStateContainer = observer(
class PaymentDataStateContainer extends React.Component {

  ccNo = ''
  name = ''
  month = ''
  year = ''
  cvv = ''
  errorMessages = {}

  handleInputChange = event => {
    const {value, name} = event.target;
    this[name] = value;
  }

  handleErrorValidationMessages = errorMessages => {
    for (const key in errorMessages) {
      this[key] = errorMessages[key];
    }
  }
  
	render() {
	  return(
      <PaymentDataForm
        method={this.props.method}
        ccNo={this.ccNo}
        name={this.name}
        month={this.month}
        year={this.year}
        cvv={this.cvv}
        onSubmit={this.onSubmitCreditCardForm}
      />
	  );
	}
}
decorate(PaymentDataStateContainer, {
  ccNo: observable,
  name: observable,
  month: observable,
  year: observable,
  cvv: observable,
  errorMessages: observable,
  handleInputChange: action,
  handleErrorValidationMessages: action,
});
