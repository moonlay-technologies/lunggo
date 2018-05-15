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
    const discountVoucherCode = '';
    const discountVoucherAmount = '';
    this.state = {
      method: null,
      creditBalance: 0,
      discountVoucherAmount,
      discountVoucherCode,
      voucherErrorMessage: '',
      isLoadingCreditBalance: false,
      isLoadingDiscountVoucher: false,
      chosenDiscountOption: '',
      pricingDetails: [...props.pricingDetails],
    };
    this.onChangedVoucherCode.bind(this);
  }

  selectMethod = method => this.setState({ method });

  applyDiscountVoucher = () => {
    const { discountVoucherCode, discountVoucherAmount } = this.state;
    this.setState({ isLoadingDiscountVoucher: true });
    checkVoucher(this.props.cartId, discountVoucherCode)
      .then(r => {
        if (r.status === 200) {
          const discountVoucherAmount = r.discount;
          const pricingDetails = [
            ...this.props.pricingDetails,
            { name: discountVoucherCode, price: (-1) * discountVoucherAmount }
          ];
          this.setState({ discountVoucherAmount, pricingDetails });
        }
        else this.setState({
          voucherErrorMessage: r.message,
          discountVoucherAmount: '',
          pricingDetails: this.props.pricingDetails,
        });
      })
      .finally(() => this.setState({ isLoadingDiscountVoucher: false }));
  }

  onChangedVoucherCode = e => {
    this.setState({ discountVoucherCode: e.target.value, voucherErrorMessage: '' });
  }

  onChangedDiscountOption = opt => {
    const { creditBalance, discountVoucherAmount, discountVoucherCode } = this.state;
    let pricingDetails = [...this.props.pricingDetails];
    if (this.state.chosenDiscountOption == opt) {
      //turn off
      opt = '';
    } else if (opt == 'creditBalance') {
      creditBalance && pricingDetails.push({ name: 'kredit referal', price: -1 * creditBalance });
    } else {
      discountVoucherAmount && pricingDetails.push({ name: discountVoucherCode, price: -1 * discountVoucherAmount });
    }
    this.setState({ pricingDetails, chosenDiscountOption: opt });
  }

  componentDidMount() {
    this.setState({ isLoadingCreditBalance: true });
    getCreditBalance(this.props.cartId)
      .then(r => {
        if (r.status === 200) this.setState({ creditBalance: r.discount });
      }).finally(() => this.setState({ isLoadingCreditBalance: false }));
  }

  render() {
    const totalPrice = this.state.pricingDetails.reduce((total, detail) => total + detail.price, 0);
    return (
      <Layout
        method={this.state.method}
        creditBalance={this.state.creditBalance}
        discountVoucherAmount={this.state.discountVoucherAmount}
        discountVoucherCode={this.state.discountVoucherCode}
        isLoadingCreditBalance={this.state.isLoadingCreditBalance}
        voucherErrorMessage={this.state.voucherErrorMessage}
        chosenDiscountOption={this.state.chosenDiscountOption}
        pricingDetails={this.state.pricingDetails}

        totalPrice={totalPrice}

        selectMethod={this.selectMethod}
        onChangedVoucherCode={this.onChangedVoucherCode}
        applyDiscountVoucher={this.applyDiscountVoucher}
        onChangedDiscountOption={this.onChangedDiscountOption}

        cartId={this.props.cartId}
        headerTitle={this.props.headerTitle}
        refund={this.props.refund}
        originalPrice={this.props.originalPrice}
        termsUrl={this.props.termsUrl}
        privacyUrl={this.props.privacyUrl}
      // mandiriClickpayToken={this.props.mandiriClickpayToken
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
  <PaymentPageStateContainer cartId={cartId} pricingDetails={pricingDetails} refund={refund} />,
  document.getElementById("react")
);