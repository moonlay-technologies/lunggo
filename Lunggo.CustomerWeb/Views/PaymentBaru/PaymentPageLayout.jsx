'use strict';
import React from 'react';
import PopUpModal from './PaymentModalStateContainer';

export default function PaymentPageLayout(props) {
  return(
    <div>
    <PopUpModal {...props} />
    {/*<!-- Navigation -->*/}
    <nav class="mynav">
      <div className="row" style="display: flex; align-items: center;">
        <div className="col-xs-1 no-padding-left"><i className="icon ion-android-arrow-back icon-pembayaran-tertiary"></i></div>
        <div className="col-xs-6  no-padding-left">{props.headerTitle}</div>
      </div>
    </nav>

    {/*<!-- Page Content -->*/}
    <div class="mother-container">

      <div class="row">
        <div class="col-xs-6 text-center no-padding-left">
          <div class="btn-potonganharga selected-discount">
            <p>Gunakan Credit</p>
            <span>{props.creditBalance}</span>
          </div>
        </div>
        <div class="col-xs-6 text-center no-padding-right">
          <div class="btn-potonganharga">
            <p>Gunakan Voucher</p>
            <span class="sub-info">masukan kode voucher disini</span>
          </div>
        </div>
      </div>

      <div class="section-container">
        <div class="section-label">Rincian Harga</div>
          <div class="row">
            <div class="col-xs-12 info-container">

              {props.pricingDetails.map( detail =>
                <div class="row" style="margin-bottom: 5px !important">
                  <div class="col-xs-6 no-padding-left">
                    <div class="info-biaya">{detail.name}</div>
                  </div>
                  <div class="col-xs-6 text-right no-padding-right">
                    <div class="info-biaya">{detail.price}</div>
                  </div>
                </div>
              )}

              <div class="row total-container">
                <div class="col-xs-6 no-padding-left">
                  <div class="info-total">Total</div>
                </div>
                <div class="col-xs-6 text-right no-padding-right">
                  <div class="info-total">Rp 899.978</div>
                </div>
              </div>


            </div>
        </div>
      </div>

      <div class="section-container">
        <div class="section-label">Pembayaran yang terakhir digunakan</div>

        <div class="row">
            <PaymentSelection text="Transfer Bank" href="#transferbank" collapsible icon={<i class="icon ion-cash icon-pembayaran-primary" />} />
            {/*style="margin-bottom: 0"*//*ini harusnya dipake buat bank transfer, nanti gw implement blm smpet*/}
        </div>

        <div class="row">
          <div class="collapse" id="transferbank" style="margin-top:5px">
            <div class="btn-method-transfer-dropdown clearfix">

              <PaymentSelection text="Bank Mandiri" href="#tfmandiri" isChild icon={<img class="img-pembayaran" src="images/mandiri.png" />} />
              <PaymentSelection text="Bank BCA" href="#tfbca" isChild icon={<img class="img-pembayaran" src="images/bca.png" />} />
              <PaymentSelection text="Bank BNI" href="#tfbni" isChild icon={<img class="img-pembayaran" src="images/bni.png" />} />
              <PaymentSelection text="Bank Danamon" href="#tfdanamon" isChild icon={<img class="img-pembayaran" src="images/danamon.png" />} />
              <PaymentSelection text="Bank CIMB Niaga" href="#tfcimbniaga" isChild icon={<img class="img-pembayaran" src="images/cimbniaga.png" />} />
              <PaymentSelection text="Bank Permata" href="#tfpermata" isChild icon={<img class="img-pembayaran" src="images/permata.png" />} />

            </div>
          </div>
        </div>
      </div>

      <div class="section-container">
        <div class="section-label">Metode Pembayaran</div>
        <div class="row">
            <PaymentSelection text="Kartu Kredit/Debit" href="#card" icon={<img class="img-pembayaran" src="images/kredit.png" />} />
            <PaymentSelection text="BCA Klikpay" href="#bcaklikpay" icon={<img class="img-pembayaran" src="images/bcaklikpay.png" />} />

            <div class="row">
              <PaymentSelection text="Debit Online" href="#debitonline" collapsible icon={<i class="icon ion-card icon-pembayaran-primary" />} />
            </div>
            <div class="row">
              <div class="collapse" id="debitonline" style="margin-top:-5px">
                <div class="btn-method-transfer-dropdown clearfix">

                  <PaymentSelection text="CIMB Clicks" href="#cimbclicks" isChild icon={<img class="img-pembayaran" src="images/cimbclick.png" />} />
                  <PaymentSelection text="Mandiri Clickpay" href="#mandiriclickpay" isChild icon={<img class="img-pembayaran" src="images/mandiriclickpay.png" />} />
                  <PaymentSelection text="e-pay BRI" href="#epaybri" isChild icon={<img class="img-pembayaran" src="images/epaybri.png" />} />
                  <PaymentSelection text="BTN Mobile Banking" href="#btnmobile" isChild icon={<img class="img-pembayaran" src="images/btn.png" />} />
                  <PaymentSelection text="IB Muamalat" href="#muamalat" isChild icon={<img class="img-pembayaran" src="images/muamalat.png" />} />
                  <PaymentSelection text="PermataNet" href="#permatanet" isChild icon={<img class="img-pembayaran" src="images/permata.png" />} />
                  
                </div>
              </div>
            </div>

            <div class="row">
              <PaymentSelection text="E-Wallet" href="#ewallet" collapsible icon={<img src="images/dompet.png" />} />
            </div>

            <div class="row">
              <div class="collapse" id="ewallet" style="margin-top:-5px">
                <div class="btn-method-transfer-dropdown clearfix">
                  <PaymentSelection text="GO-PAY" href="#gopay" isChild icon={<img class="img-pembayaran" src="images/gopay.png" />} />
                </div>
              </div>
            </div>

        </div>
      </div>


    </div>
    </div>{/* <!-- end mother-container -->

    <!-- Bootstrap core JavaScript -->
    <script src="js/jquery.js"></script>
    <script src="js/bootstrap.js"></script>*/}

  );
}

function PaymentSelection(props) {
  const collapsibleAttributes = {
    style:{marginBottom: 10},
    dataToggle: 'collapse'
  }
  const className = 'col-xs-12 btn-method-transfer' +
    props.isChild ? '-dropdown-child' : '';
  return(
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
