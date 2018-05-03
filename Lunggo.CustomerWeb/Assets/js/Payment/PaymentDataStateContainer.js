'use strict';
import React from 'react';
//import { observable, action, decorate } from "mobx";
//import { observer } from "mobx-react";
import PaymentDataForm from './PaymentDataForm.jsx';

//const PaymentDataStateContainer = observer(
class PaymentDataStateContainer extends React.Component {

  constructor() {
    super();
    this.state = {
      ccNo: '',
      name: '',
      month: '',
      year: '',
      cvv: '',
      errorMessages: {},
    }
  }

  handleInputChange = event => {
    const { value, name } = event.target;
    this.setState({ [name]: value });
  }

  handleErrorValidationMessages = errorMessages => {
    for (const key in errorMessages) {
      this.setState({ [key]: errorMessages[key] });
    }
  }

  render() {
    return (
      <PaymentDataForm
        method={this.props.method}
        ccNo={this.state.ccNo}
        name={this.state.name}
        month={this.state.month}
        year={this.state.year}
        cvv={this.state.cvv}
        onSubmit={this.onSubmitCreditCardForm}
      />
    );
  }
}
//);

//decorate(PaymentDataStateContainer, {
//  ccNo: observable,
//  name: observable,
//  month: observable,
//  year: observable,
//  cvv: observable,
//  errorMessages: observable,
//  handleInputChange: action,
//  handleErrorValidationMessages: action,
//});

export default PaymentDataStateContainer;