'use strict';
import React from 'react';
import { observable, action } from "mobx";
import { observer } from "mobx-react";
import Layout from './PaymentPage';
import { getCreditBalance, sumTotalBill } from './PaymentController';

@observer export default
class PaymentPageStateContainer extends React.Component {

  @observable method = null
  @observable creditBalance = ''
  @observable errorMessage = ''
  @observable isLoadingCreditBalance = false

  @action setMethod(method) { this.method = method }

  @action componentDidMount() {
    this.isLoadingCreditBalance = true;
    getCreditBalance().then( r => {
      if (r.status=200) this.creditBalance = r.discount;
      else this.errorMessage = r.error;
    }).finally( () => this.isLoadingCreditBalance = false);
  }

  applyDiscountVoucher = () => {
    this.isLoadingCreditBalance = true;
    getCreditBalance().then( r => {
      if (r.status=200) this.creditBalance = r.discount;
      else this.errorMessage = r.error;
    }).finally( () => this.isLoadingCreditBalance = false);
  }

	render() {
	  return(
      <Layout
        method={this.method}
        setMethod={this.setMethod}

        rsvNo={this.props.rsvNo}
        discCd={this.props.discCd}
        headerTitle={this.props.headerTitle}
        creditBalance={this.props.creditBalance}
        pricingDetails={this.props.pricingDetails}
        refund='tidak bisa refund untuk aktivitas ini'
        originalPrice={this.props.originalPrice}
        termsUrl={this.props.termsUrl}
        privacyUrl={this.props.privacyUrl}
        // mandiriClickpayToken={this.props.mandiriClickpayToken
        // cartId='0'
      />
	  );
	}
}