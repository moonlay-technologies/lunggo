'use strict';
import React from 'react';
import { observable, action } from "mobx";
import { observer } from "mobx-react";
import Layout from './PaymentModalLayout';
import { pay } from './PaymentController';

@observer export default
class PaymentModalStateContainer extends React.Component {

  @observable isLoading = false
  @observable errorMessage = ''
  // @observable errorMessages = {}

  @action
  onSubmitCreditCardForm = () => {
    const { ccNo, name, month, year, cvv,
      handleErrorValidationMessages } = this.formState;
    const formData = { ccNo, name, cvv, expiry: {month, year} };
    this.isLoading = true;
    pay( { ...this.props, formData}, handleErrorValidationMessages)
      .then( res => this.errorMessage = res )
      .finally( () => this.isLoading = false );
  }

  getFormState = e => this.formState = e;
  
	render() {
	  return(
      <Layout
        method={this.props.method}
        ccNo={this.ccNo}
        name={this.name}
        month={this.month}
        year={this.year}
        cvv={this.cvv}
        onSubmit={this.onSubmitCreditCardForm}
        bindFormRef={this.getFormState}
      />
	  );
	}
}