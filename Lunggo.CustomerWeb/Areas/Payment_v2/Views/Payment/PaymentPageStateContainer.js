'use strict';
import React from 'react';
import { observable, action } from "mobx";
import { observer } from "mobx-react";
import Layout from './PaymentPage';
import { getCreditBalance, sumTotalBill } from './PaymentController';

@observer export default
class PaymentPageStateContainer extends React.Component {

  @observable method = null
  @observable creditBalance = this.props.creditBalance
  @observable discountVoucherAmount = ''
  @observable discountVoucherCode = ''
  @observable errorMessage = ''
  @observable isLoadingCreditBalance = false
  @observable isLoadingDiscountVoucher = false

  @action setMethod(method) { this.method = method }

  @action componentDidMount() {
    this.isLoadingCreditBalance = true;
    getCreditBalance()
      .then( r => {
        if (r.status=200) this.creditBalance = r.discount;
        else this.errorMessage = r.error;
      })
      .finally( () => this.isLoadingCreditBalance = false);
  }

  @action applyDiscountVoucher = () => {
    this.isLoadingDiscountVoucher = true;
    getCreditBalance()
      .then( r => {
        if (r.status=200) this.discountVoucherAmount = r.discount;
        else this.errorMessage = r.error;
      })
      .finally( () => this.isLoadingDiscountVoucher = false);
  }

  @action onChangedVoucherCode = e => {
    this.discountVoucherCode = e.target.value;
  }

	render() {
	  return(
      <Layout
        method={this.method}
        setMethod={this.setMethod}
        creditBalance={this.creditBalance}
        discountVoucherAmount={this.discountVoucherAmount}
        discountVoucherCode={this.discountVoucherCode}
        onChangedVoucherCode={this.onChangedVoucherCode}
        applyDiscountVoucher={this.applyDiscountVoucher}

        rsvNo={this.props.rsvNo}
        discCd={this.props.discCd}
        headerTitle={this.props.headerTitle}
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