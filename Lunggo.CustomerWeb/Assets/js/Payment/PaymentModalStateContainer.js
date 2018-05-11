'use strict';
import React from 'react';
//import { observable, action, decorate } from "mobx";
//import { observer } from "mobx-react";
import Layout from './PaymentModalLayout.jsx';
import { pay } from './PaymentController';

//const PaymentModalStateContainer = observer(
class PaymentModalStateContainer extends React.Component {
  constructor() {
    super();
    this.state = {
      isLoading: false,
      errorMessage: '',
      paymentStep: 'initial',
      iframeUrl: '',

      ccNo: '',
      name: '',
      expiry:'',
      //month: '',
      //year: '',
      cvv: '',
      errorMessages: {},
    };
    this.handleInputChange = this.handleInputChange.bind(this);
  }

  handleInputChange = event => {
    const { value, name } = event.target;
    const errorMessages = { ...this.state.errorMessages, [name]: '' };
    this.setState({ [name]: value, errorMessages });
  }

  handleErrorValidationMessages = errorMessages => {
    this.setState({ errorMessages });
  }

  changePaymentStepLayout = (paymentStep, paymentStepStringData = '') => {
    this.setState({ paymentStep, paymentStepStringData });
  }

  onSubmitCreditCardForm = () => {
    //e.preventDefault();
    const { ccNo, name, cvv, expiry } = this.state;
    const formData = { ccNo, name, cvv, expiry };
    this.setState({ isLoading: true });
    pay({ ...this.props, formData, discCd: this.props.discountVoucherCode }, this.handleErrorValidationMessages, this.changePaymentStepLayout)
      //.then(res => /*this.setState({ errorMessage: res })*/ console.log('pay resolved',res) )
      //.finally(() => /*this.setState({ isLoading: false })*/ console.log('pay ended') );
  }

  backToMethodSelection = () => {
    this.changePaymentStepLayout('initial');
  }

  backToMyBookings = () => {
    window.postMessage('backToMyBookings');
    //// navigate web to myBooking page
  }

  render() {
    return (
      <Layout
        method={this.props.method}
        ccNo={this.state.ccNo}
        name={this.state.name}
        expiry={this.state.expiry}
        //month={this.state.month}
        //year={this.state.year}
        cvv={this.state.cvv}
        errorMessages={this.state.errorMessages}
        paymentStep={this.state.paymentStep}
        paymentStepStringData={this.state.paymentStepStringData}
        onSubmit={this.onSubmitCreditCardForm}
        handleInputChange={this.handleInputChange}
        shouldShowDataForm={this.props.method == 'card' || this.props.method == 'mandiriClickPay'}
        discountVoucherCode={this.props.discountVoucherCode}
        backToMyBookings={this.backToMyBookings}
        backToMethodSelection={this.backToMethodSelection}
      />
    );
  }
}
//);

//decorate(PaymentModalStateContainer, {
//  isLoading: observable,
//  errorMessage: observable,
//  onSubmitCreditCardForm: action,
//});

export default PaymentModalStateContainer;