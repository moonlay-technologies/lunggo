'use strict';
import React from 'react';
import ReactDOM from 'react-dom';
//import { observable, action, decorate } from "mobx";
//import { observer } from "mobx-react";
import Layout from './PaymentPageLayout.jsx';
import { getCreditBalance, checkVoucher, sumTotalBill } from './PaymentController';

//const PaymentPageStateContainer = observer(
class PaymentPageStateContainer extends React.Component {


  constructor(props) {
    super(props);
    this.state = {
      method: null,
      creditBalance: `loading...`,
      discountVoucherAmount: '',
      discountVoucherCode: '',
      voucherErrorMessage: '',
      isLoadingCreditBalance: false,
      isLoadingDiscountVoucher: false,
      chosenDiscountOption: 'creditBalance',
    };
  }

  selectMethod = method => this.setState({ method });

  applyDiscountVoucher = () => {
    this.setState({ isLoadingDiscountVoucher: true });
    checkVoucher(this.props.cartId, this.state.discountVoucherCode)
      .then(r => {
        if (r.status === 200) this.setState({ discountVoucherAmount: r.discount });
        else this.setState({ voucherErrorMessage: r.message, discountVoucherAmount: '' });
      })
      .finally(() => this.setState({ isLoadingDiscountVoucher: false }));
  }

  onChangedVoucherCode = e => {
    this.setState({ discountVoucherCode: e.target.value, voucherErrorMessage: '' });
  }

  onChangedDiscountOption = opt => this.setState({ chosenDiscountOption: opt });

  componentDidMount() {
    this.setState({ isLoadingCreditBalance: true });
    getCreditBalance(this.props.cartId)
      .then(r => {
        if (r.status === 200) this.setState({ creditBalance: r.discount });
        // else this.setState({ errorMessage: r.message });
      })
      .finally(() => this.setState({ isLoadingCreditBalance: false }));
  }

  render() {
    return (
      <Layout
        method={this.state.method}
        creditBalance={this.state.creditBalance}
        discountVoucherAmount={this.state.discountVoucherAmount}
        discountVoucherCode={this.state.discountVoucherCode}
        isLoadingCreditBalance={this.state.isLoadingCreditBalance}
        voucherErrorMessage={this.state.voucherErrorMessage}
        chosenDiscountOption={this.state.chosenDiscountOption}

        selectMethod={this.selectMethod}
        onChangedVoucherCode={this.onChangedVoucherCode}
        applyDiscountVoucher={this.applyDiscountVoucher}
        onChangedDiscountOption={this.onChangedDiscountOption}

        cartId={this.props.cartId}
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
  <PaymentPageStateContainer cartId={cartId} />,
  document.getElementById("react")
);