'use strict';
import React from 'react';

function PaymentInstructionLayout(props) {
    return (
        <div className="section-container">
            <div className="section-label">Instruksi Pembayaran</div>
            <div className="row">
                <div className="col-xs-12 info-container">
                    <div className="info-transfer">
                        <ol>
                            <li>Gunakan ATM/Mbanking/Setor Tunai untuk transfer</li>
                            <li>No. Rek: 12344565767676766<br />Cabang: Senayan <br />Nama Rekening: PT. Travel Madezy</li>
                            <li style={{ margin: 0 }}>Lakukan pembayaran sebelum tanggal<br />dd/mm/yy : 10.00 PM</li>
                        </ol>
                    </div>
                </div>
            </div>
        </div>
    );
}
export default PaymentInstructionLayout;