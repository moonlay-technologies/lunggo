'use strict';
import React from 'react';
//import { observable, action, decorate } from "mobx";
//import { observer } from "mobx-react";
import PaymentDataForm from './PaymentDataForm.jsx';

//const PaymentDataStateContainer = observer(
class PaymentDataStateContainer extends React.Component {

  //constructor() {
  //  super();
  //  this.state = {
  //    ccNo: '',
  //    name: '',
  //    month: '',
  //    year: '',
  //    cvv: '',
  //    errorMessages: {},
  //  }
  //}

  //handleInputChange = event => {
  //  const { value, name } = event.target;
  //  this.setState({ [name]: value });
  //}

  //handleErrorValidationMessages = errorMessages => {
  //  for (const key in errorMessages) {
  //    this.setState({ [key]: errorMessages[key] });
  //  }
  //}

  render() {
    return (
      <PaymentDataForm
        method={this.props.method}
        ccNo={this.props.ccNo}
        name={this.props.name}
        month={this.props.month}
        year={this.props.year}
        cvv={this.props.cvv}
        errorMessages={this.props.errorMessages}
        handleInputChange={this.props.handleInputChange}
        {...this.props}
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