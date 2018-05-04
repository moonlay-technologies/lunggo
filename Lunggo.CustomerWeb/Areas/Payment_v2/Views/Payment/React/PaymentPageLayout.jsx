'use strict';
import React from 'react';
import PopUpModal from './PaymentModalStateContainer';

function PaymentPageLayout(props) {
    return (
        <div>
            <PopUpModal {...props} />
            {/*<!-- Navigation -->*/}
            <nav className="mynav">
                <div className="row" style={{display: 'flex', alignTtems: 'center'}}>
                    <div className="col-xs-1 no-padding-left"><i className="icon ion-android-arrow-back icon-pembayaran-tertiary"></i></div>
                    <div className="col-xs-6 no-padding-left">{props.headerTitle}</div>
                </div>
            </nav>

            {/*<!-- Page Content -->*/}
            <div className="mother-container">

                <div className="row">
                    <div className="col-xs-6 text-center no-padding-left">
                        <div className="btn-potonganharga selected-discount">
                            <p>Gunakan Credit</p>
                            <span>{props.creditBalance}</span>
                        </div>
                    </div>
                    <div className="col-xs-6 text-center no-padding-right" data-toggle="collapse" href="#kodevoucher">
                        <div className="btn-potonganharga">
                            <p>Gunakan Voucher</p>
                            <span className="sub-info">masukan kode voucher disini</span>
                        </div>
                    </div>
                </div>

                <div className="section-container collapse" id="kodevoucher">
                    <div className="row">
                        <div className="col-xs-12 info-container">
                            <form>
                                <div className="form-group">
                                    <label className="label-form" for="nokartu">Kode Voucher</label>
                                    <input value={props.discountVoucherCode} onChange={props.onChangedVoucherCode} type="number" className="form-control form-payment validation-form-true" id="nokartu" placeholder="Masukkan kode voucher disini" />
                                    <div className="text-validation">Kode salah</div>
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

                            {/*}{props.pricingDetails.map(detail =>
                                <div className="row" style="margin-bottom: 5px !important">
                                    <div className="col-xs-6 no-padding-left">
                                        <div className="info-biaya">{detail.name}</div>
                                    </div>
                                    <div className="col-xs-6 text-right no-padding-right">
                                        <div className="info-biaya">{detail.price}</div>
                                    </div>
                                </div>
                            )}*/}

                            <div className="row total-container">
                                <div className="col-xs-6 no-padding-left">
                                    <div className="info-total">Total</div>
                                </div>
                                <div className="col-xs-6 text-right no-padding-right">
                                    <div className="info-total">Rp 899.978</div>
                                </div>
                            </div>


                        </div>
                    </div>
                </div>

                <div className="section-container">
                    <div className="section-label">Pembayaran yang terakhir digunakan</div>

                    <div className="row">
                        <PaymentSelection text="Transfer Bank" href="#transferbank" collapsible icon={<i className="icon ion-cash icon-pembayaran-primary" />} />
                        {/*style="margin-bottom: 0"*//*ini harusnya dipake buat bank transfer, nanti gw implement blm smpet*/}
                    </div>

                    <div className="row">
                        <div className="collapse" id="transferbank" style={{ marginTop: 5 }}>
                            <div className="btn-method-transfer-dropdown clearfix">

                                <PaymentSelection text="Bank Mandiri" href="#tfmandiri" isChild icon={<img className="img-pembayaran" src="/Assets/images/bank/mandiri.png" />} />
                                <PaymentSelection text="Bank BCA" href="#tfbca" isChild icon={<img className="img-pembayaran" src="/Assets/images/bank/bca.png" />} />
                                <PaymentSelection text="Bank BNI" href="#tfbni" isChild icon={<img className="img-pembayaran" src="/Assets/images/bank/bni.png" />} />
                                <PaymentSelection text="Bank Danamon" href="#tfdanamon" isChild icon={<img className="img-pembayaran" src="/Assets/images/bank/danamon.png" />} />
                                <PaymentSelection text="Bank CIMB Niaga" href="#tfcimbniaga" isChild icon={<img className="img-pembayaran" src="/Assets/images/bank/cimbniaga.png" />} />
                                <PaymentSelection text="Bank Permata" href="#tfpermata" isChild icon={<img className="img-pembayaran" src="/Assets/images/bank/permata.png" />} />

                            </div>
                        </div>
                    </div>
                </div>

                <div className="section-container">
                    <div className="section-label">Metode Pembayaran</div>
                    <div className="row">
                        <PaymentSelection text="Kartu Kredit/Debit" href="#card" icon={<img className="img-pembayaran" src="/Assets/images/bank/kredit.png" />} />
                        <PaymentSelection text="BCA Klikpay" href="#bcaklikpay" icon={<img className="img-pembayaran" src="/Assets/images/bank/bcaklikpay.png" />} />

                        <div className="row">
                            <PaymentSelection text="Debit Online" href="#debitonline" collapsible icon={<i className="icon ion-card icon-pembayaran-primary" />} />
                        </div>
                        <div className="row">
                            <div className="collapse" id="debitonline" style={{ marginTop: -5 }}>
                                <div className="btn-method-transfer-dropdown clearfix">

                                    <PaymentSelection text="CIMB Clicks" href="#cimbclicks" isChild icon={<img className="img-pembayaran" src="/Assets/images/bank/cimbclick.png" />} />
                                    <PaymentSelection text="Mandiri Clickpay" href="#mandiriclickpay" isChild icon={<img className="img-pembayaran" src="/Assets/images/bank/mandiriclickpay.png" />} />
                                    <PaymentSelection text="e-pay BRI" href="#epaybri" isChild icon={<img className="img-pembayaran" src="/Assets/images/bank/epaybri.png" />} />
                                    <PaymentSelection text="BTN Mobile Banking" href="#btnmobile" isChild icon={<img className="img-pembayaran" src="/Assets/images/bank/btn.png" />} />
                                    <PaymentSelection text="IB Muamalat" href="#muamalat" isChild icon={<img className="img-pembayaran" src="/Assets/images/bank/muamalat.png" />} />
                                    <PaymentSelection text="PermataNet" href="#permatanet" isChild icon={<img className="img-pembayaran" src="/Assets/images/bank/permata.png" />} />

                                </div>
                            </div>
                        </div>

                        <div className="row">
                            <PaymentSelection text="E-Wallet" href="#ewallet" collapsible icon={<img src="/Assets/images/bank/dompet.png" />} />
                        </div>

                        <div className="row">
                            <div className="collapse" id="ewallet" style={{ marginTop: -5 }}>
                                <div className="btn-method-transfer-dropdown clearfix">
                                    <PaymentSelection text="GO-PAY" href="#gopay" isChild icon={<img className="img-pembayaran" src="/Assets/images/bank/gopay.png" />} />
                                </div>
                            </div>
                        </div>

                    </div>
                </div>


            </div>
        </div>
  );
}

function PaymentSelection(props) {
    const collapsibleAttributes = {
        style: { marginBottom: 10 },
        dataToggle: 'collapse'
    }
    const className = 'col-xs-12 btn-method-transfer' +
        (props.isChild ? '-dropdown-child' : '');
    return (
        <a href={props.href} className={className}
            {...props.collapsible ? collapsibleAttributes : {} }
        >
            <div className="col-xs-3 no-padding-left">
                {props.icon}
            </div>
            <div className="col-xs-7 no-padding">
                <div className="info-total">{props.text}</div>
            </div>
            <div className="col-xs-2 no-padding-right text-right">
                <i className="icon ion-chevron-right icon-pembayaran-tertiary"></i>
            </div>
        </a>
    );
}

export default PaymentPageLayout;