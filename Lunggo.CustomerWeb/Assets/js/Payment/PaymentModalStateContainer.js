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
    }
  }

  onSubmitCreditCardForm = () => {
    const { ccNo, name, month, year, cvv,
      handleErrorValidationMessages } = this.state.formState;
    const formData = { ccNo, name, cvv, expiry: { month, year } };
    this.setState({ isLoading: true });
    pay({ ...this.props, formData }, handleErrorValidationMessages)
      .then(res => this.setState({ errorMessage: res }))
      .finally(() => this.setState({ isLoading: false }));
  }

  getFormState = e => this.setState({ formState: e });

  render() {
    // return ( props.showModal &&
    return (
      <Layout
        method={this.props.method}
        ccNo={this.state.ccNo}
        name={this.state.name}
        month={this.state.month}
        year={this.state.year}
        cvv={this.state.cvv}
        onSubmit={this.onSubmitCreditCardForm}
        bindFormRef={this.getFormState}
      />
    );
  }
}
);

//decorate(PaymentModalStateContainer, {
//  isLoading: observable,
//  errorMessage: observable,
//  onSubmitCreditCardForm: action,
//});

export default PaymentModalStateContainer;