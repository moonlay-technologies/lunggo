'use strict';
import React from 'react';
import ReactDOM from 'react-dom';
//import { observable, action, decorate } from "mobx";
//import { observer } from "mobx-react";
import Layout from './PaymentPageLayout.jsx';
import { getCreditBalance, sumTotalBill } from './PaymentController';

//const PaymentPageStateContainer = observer(
class PaymentPageStateContainer extends React.Component {


  constructor() {
    super();
    this.state = {
      method: null,
      creditBalance: this.props.creditBalance,
      discountVoucherAmount: '',
      discountVoucherCode: '',
      errorMessage: '',
      isLoadingCreditBalance: false,
      isLoadingDiscountVoucher: false,
      //showModal: false,
    };
  }

  selectMethod(method) {
    this.setState({ method });
    //this.showModal = true;
    console.log('from select method', this.state.method)
  }

  applyDiscountVoucher = () => {
    this.setState({ isLoadingDiscountVoucher: true });
    getCreditBalance()
      .then(r => {
        if (r.status === 200) this.setState({ discountVoucherAmount: r.discount });
        else this.setState({ errorMessage: r.error });
      })
      .finally(() => this.setState({ isLoadingDiscountVoucher: false }));
  }

  onChangedVoucherCode = e => {
    this.setState({ discountVoucherCode: e.target.value });
  }

  componentDidMount() {
    this.setState({ isLoadingCreditBalance: true });
    getCreditBalance()
      .then(r => {
        if (r.status === 200) this.setState({ creditBalance: r.discount });
        else this.setState({ errorMessage: r.error });
      })
      .finally(() => this.setState({ isLoadingCreditBalance: false }));
  }

  render() {
    console.log('from render', this.state.method)
    return (
      <Layout
        method={this.state.method}
        selectMethod={this.state.selectMethod}
        creditBalance={this.state.creditBalance}
        discountVoucherAmount={this.state.discountVoucherAmount}
        discountVoucherCode={this.state.discountVoucherCode}
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
//);

//decorate(PaymentPageStateContainer, {
//  PaymentPageStateContainer: observer,
//  method: observable,
//  creditBalance: observable,
//  discountVoucherAmount: observable,
//  discountVoucherCode: observable,
//  errorMessage: observable,
//  isLoadingCreditBalance: observable,
//  isLoadingDiscountVoucher: observable,
//  showModal: observable,
//  selectMethod: action,
//  componentDidMount: action,
//  applyDiscountVoucher: action,
//  onChangedVoucherCode: action
//});

export default PaymentPageStateContainer;

ReactDOM.render(
  <PaymentPageStateContainer />,
  document.getElementById("react")
);