'use strict';
import React from 'react';
import { observable, action } from "mobx";
import { observer } from "mobx-react";
import PaymentDataForm from './PaymentDataForm';

@observer export default
class PaymentDataStateContainer extends React.Component {

  @observable ccNo = ''
  @observable name = ''
  // @observable expiryDate = {month:'', year: ''}
  @observable month = ''
  @observable year = ''
  @observable cvv = ''
  @observable errorMessages = {}

  @action
  handleInputChange = event => {
    const {value, name} = event.target;
    this[name] = value;
  }

  @action
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