'use strict';
import React from 'react';
import PopUpModal from './PaymentModalStateContainer';

//import DevTools from 'mobx-react-devtools';

//// Format number to "1.000.000"
var number = int => int.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");

//// Format price to "Rp 1.000.000"
var rupiah = int => {
  const numStr = number(int);
  if (numStr.substr(0,1)=='-') return '–Rp ' + numStr.substr(1);
  else return 'Rp ' + numStr;
}

function PaymentPageLayout(props) {
  return (
    <div>
      <PopUpModal {...props} />
      {/*<!-- Navigation -->*/}
      <nav className="mynav">
        <div className="row" style={{ display: 'flex', alignTtems: 'center' }}>
          <div className="col-xs-1 no-padding-left"><i className="icon ion-android-arrow-back icon-pembayaran-tertiary"></i></div>
          <div className="col-xs-6 no-padding-left">{props.headerTitle}</div>
        </div>
      </nav>

      {/*<!-- Page Content -->*/}
      <div className="mother-container">

        <div className="row">
          <div className="col-xs-6 text-center no-padding-left">
            <a href="#" onClick={() => props.onChangedDiscountOption('creditBalance')}>
            <div className={props.chosenDiscountOption == 'creditBalance' ? `btn-potonganharga selected-discount` : `btn-potonganharga`}
              
            >
              <p>Gunakan Credit</p>
              <span>{props.creditBalance}</span>
              </div>
              </a>
          </div>
          <div className="col-xs-6 text-center no-padding-right" data-toggle="collapse" href="#kodevoucher">
            <a href="#">
              <div className={props.chosenDiscountOption == 'voucherCode' ? `btn-potonganharga selected-discount` : `btn-potonganharga`}
              onClick={() => props.onChangedDiscountOption('voucherCode')}
            >
              <p>Gunakan Voucher</p>
              {props.discountVoucherAmount ?
                <span>{props.discountVoucherAmount}</span> :
                <span className="sub-info">masukkan kode voucher disini</span>
              }
              </div>
              </a>
          </div>
        </div>

        <div className="section-container collapse" id="kodevoucher">
          <div className="row">
            <div className="col-xs-12 info-container">
              <form>
                <div className="form-group">
                  <label className="label-form" for="nokartu">Kode Voucher</label>
                  <input value={props.discountVoucherCode} onChange={props.onChangedVoucherCode} type="text" className="form-control form-payment validation-form-true" id="nokartu" placeholder="Masukkan kode voucher disini" />
                  <div className="text-validation">{props.voucherErrorMessage}</div>
                  {/*<div className="text-validation-true">Kode benar</div>*/}
                </div>

                <div className="row">
                  <div className="col-xs-12 no-padding">
                    <a href="#" className="button-primary" onClick={props.applyDiscountVoucher}>Gunakan</a>
                  </div>
                </div>

              </form>
            </div>
          </div>
        </div>

        <div className="section-container">
          <div className="section-label">Rincian Harga</div>
          <div className="row">
            <div className="col-xs-12 info-container">

              {props.pricingDetails.map(detail =>
                  <div className="row" style={{ marginBottom: "5px !important" }}>
                      <div className="col-xs-6 no-padding-left">
                          <div className="info-biaya">{detail.name}</div>
                      </div>
                      <div className="col-xs-6 text-right no-padding-right">
                          <div className="info-biaya">{rupiah(detail.price)}</div>
                      </div>
                  </div>
              )}

              <div className="row total-container">
                <div className="col-xs-6 no-padding-left">
                  <div className="info-total">Total</div>
                </div>
                <div className="col-xs-6 text-right no-padding-right">
                  <div className="info-total">{rupiah(props.totalPrice)}</div>
                </div>
              </div>


            </div>
          </div>
        </div>
        {/*
        <div className="section-container">
          <div className="section-label">Pembayaran yang terakhir digunakan</div>

          <div className="row">
            <PaymentSelection text="Transfer Bank" href="#transferbank" collapsible icon={<i className="icon ion-cash icon-pembayaran-primary" />}
              //style="margin-bottom: 0"*//*ini harusnya dipake buat bank transfer, nanti gw implement blm smpet
            />
          </div>
          <div className="row">
            <div className="collapse" id="transferbank" style={{ marginTop: 5 }}>
              <div className="btn-method-transfer-dropdown clearfix">
                <PaymentSelection text="Bank Mandiri" onClick={() => props.selectMethod('tfmandiri')} isChild icon={<img className="img-pembayaran" src="/Assets/images/bank/mandiri.png" />} />
                <PaymentSelection text="Bank BCA" onClick={() => props.selectMethod('tfbca')} isChild icon={<img className="img-pembayaran" src="/Assets/images/bank/bca.png" />} />
                <PaymentSelection text="Bank BNI" onClick={() => props.selectMethod('tfbni')} isChild icon={<img className="img-pembayaran" src="/Assets/images/bank/bni.png" />} />
                <PaymentSelection text="Bank Danamon" onClick={() => props.selectMethod('tfdanamon')} isChild icon={<img className="img-pembayaran" src="/Assets/images/bank/danamon.png" />} />
                <PaymentSelection text="Bank CIMB Niaga" onClick={() => props.selectMethod('tfcimbniaga')} isChild icon={<img className="img-pembayaran" src="/Assets/images/bank/cimbniaga.png" />} />
                <PaymentSelection text="Bank Permata" onClick={() => props.selectMethod('tfpermata')} isChild icon={<img className="img-pembayaran" src="/Assets/images/bank/permata.png" />} />

              </div>
            </div>
          </div>
        </div>*/}

        <div className="section-container">
          <div className="section-label">Metode Pembayaran</div>
          <div className="row">
            <PaymentSelection text="Kartu Kredit/Debit" onClick={() => props.selectMethod('card')} icon={<img className="img-pembayaran" src="/Assets/images/bank/kredit.png" />} />
          </div>
          {/*<div className="row">
            <PaymentSelection text="BCA Klikpay" onClick={() => props.selectMethod('bcaklikpay')} icon={<img className="img-pembayaran" src="/Assets/images/bank/bcaklikpay.png" />} /></div>

          <div className="row">
            <PaymentSelection text="Debit Online" href="#debitonline" collapsible icon={<i className="icon ion-card icon-pembayaran-primary" />} />
          </div>
          <div className="row">
            <div className="collapse" id="debitonline" style={{ marginTop: -5 }}>
              <div className="btn-method-transfer-dropdown clearfix">

                <PaymentSelection text="CIMB Clicks" onClick={() => props.selectMethod('cimbclicks')} isChild icon={<img className="img-pembayaran" src="/Assets/images/bank/cimbclick.png" />} />
                <PaymentSelection text="Mandiri Clickpay" onClick={() => props.selectMethod('mandiriClickPay')} isChild icon={<img className="img-pembayaran" src="/Assets/images/bank/mandiriclickpay.png" />} />
                <PaymentSelection text="e-pay BRI" onClick={() => props.selectMethod('epaybri')} isChild icon={<img className="img-pembayaran" src="/Assets/images/bank/epaybri.png" />} />
                <PaymentSelection text="BTN Mobile Banking" onClick={() => props.selectMethod('btnmobile')} isChild icon={<img className="img-pembayaran" src="/Assets/images/bank/btn.png" />} />
                <PaymentSelection text="IB Muamalat" onClick={() => props.selectMethod('muamalat')} isChild icon={<img className="img-pembayaran" src="/Assets/images/bank/muamalat.png" />} />
                <PaymentSelection text="PermataNet" onClick={() => props.selectMethod('permatanet')} isChild icon={<img className="img-pembayaran" src="/Assets/images/bank/permata.png" />} />

              </div>
            </div>
          </div>

          <div className="row">
            <PaymentSelection text="E-Wallet" href="#ewallet" collapsible icon={<img src="/Assets/images/bank/dompet.png" />} />
          </div>

          <div className="row">
            <div className="collapse" id="ewallet" style={{ marginTop: -5 }}>
              <div className="btn-method-transfer-dropdown clearfix">
                <PaymentSelection text="GO-PAY" onClick={() => props.selectMethod('gopay')} isChild icon={<img className="img-pembayaran" src="/Assets/images/bank/gopay.png" />} />
              </div>
            </div>
          </div>
          */}
        </div>


      </div>
    </div>
  );
}

function PaymentSelection(props) {
  const collapsibleAttr = {
    style: { marginBottom: 10 },
    'data-toggle': 'collapse'
  }
  const uncollapsibleAttr = {
    onClick: props.onClick,
    'data-toggle': 'modal',
    'data-target': '#payment-modal',
  }
  const className = 'col-xs-12 btn-method-transfer' +
    (props.isChild ? '-dropdown-child' : '');
  return (
    <a href={props.href || "#"} className={className}
      {...props.collapsible ? collapsibleAttr : uncollapsibleAttr }
    >
      <div className="col-xs-3 no-padding-left">
        {props.icon}
      </div>
      <div className="col-xs-7 no-padding">
        <div className="info-total">{props.text}</div>
      </div>
      <div className="col-xs-2 no-padding-right text-right">
        <i className="icon ion-chevron-right icon-pembayaran-tertiary" />
      </div>
    </a>
  );
}

export default PaymentPageLayout;