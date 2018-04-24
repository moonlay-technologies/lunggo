'use strict';
import React from 'react';
// import ShallowRenderer from 'react-test-renderer/shallow';
import {
  sumTotalBill, convertToAbsoluteNegative, validateCCVFormat,
  validateCreditCardNumberFormat, validateCreditCardExpiryDate,

} from './PaymentController';

describe('PaymentController', () => {

  it(`should make discount value a negative value`, () => {
    expect(convertToAbsoluteNegative(40)).toBe(-40);
    expect(convertToAbsoluteNegative(-40)).toBe(-40);
    expect(convertToAbsoluteNegative(892)).toBe(-892);
    expect(convertToAbsoluteNegative(-892)).toBe(-892);
    expect(convertToAbsoluteNegative(0)).toBe(0);
  });

  it(`should sum total bill`, () => {
    let actualResult = sumTotalBill(20, 30, 40, 50, convertToAbsoluteNegative(100));
    expect(actualResult).toBe(40);
    actualResult = sumTotalBill(20, 30, 40, 50, convertToAbsoluteNegative(-100));
    expect(actualResult).toBe(40);
  });

  //// CREDIT CARD
  it(`check credit card number format`, () => {
    let res = validateCreditCardNumberFormat(1234567890123456);
    expect(res).toBe(true);
    res = validateCreditCardNumberFormat('1234567890123456');
    expect(res).toBe(true);

    res = validateCreditCardNumberFormat('12345678901234ww');
    expect(res).toBe(false);
    res = validateCreditCardNumberFormat(12345678901234);
    expect(res).toBe(false);
    res = validateCreditCardNumberFormat(12345678901234567890);
    expect(res).toBe(false);
    res = validateCreditCardNumberFormat('');
    expect(res).toBe(false);
    res = validateCreditCardNumberFormat(null);
    expect(res).toBe(false);
    res = validateCreditCardNumberFormat(undefined);
    expect(res).toBe(false);
    res = validateCreditCardNumberFormat(0);
    expect(res).toBe(false);
  });

  it(`check CCV format`, () => {
    expect(validateCCVFormat(123)).toBe(true);
    expect(validateCCVFormat(1234)).toBe(true);
    expect(validateCCVFormat('123')).toBe(true);
    expect(validateCCVFormat('1234')).toBe(true);

    expect(validateCCVFormat(12345)).toBe(false);
    expect(validateCCVFormat(12)).toBe(false);
    expect(validateCCVFormat('123345')).toBe(false);
    expect(validateCCVFormat('12')).toBe(false);
    expect(validateCCVFormat('12a')).toBe(false);
    expect(validateCCVFormat('12aa')).toBe(false);
    expect(validateCCVFormat('')).toBe(false);
    expect(validateCCVFormat(null)).toBe(false);
    expect(validateCCVFormat(undefined)).toBe(false);
    expect(validateCCVFormat(0)).toBe(false);
  });

  it(`check credit card expiry format`, () => {
    expect(validateCreditCardExpiryDate(2,21)).toBe(true);
    expect(validateCreditCardExpiryDate(2,'21')).toBe(true);
    expect(validateCreditCardExpiryDate('2',21)).toBe(true);
    expect(validateCreditCardExpiryDate('02',21)).toBe(true);
    expect(validateCreditCardExpiryDate('02','21')).toBe(true);
    expect(validateCreditCardExpiryDate(2,2021)).toBe(true);
    expect(validateCreditCardExpiryDate('2','2021')).toBe(true);

    expect(validateCreditCardExpiryDate(22,21)).toBe(false);
    expect(validateCreditCardExpiryDate(2,1)).toBe(false);
    expect(validateCreditCardExpiryDate('22','21')).toBe(false);
    expect(validateCreditCardExpiryDate('2','1')).toBe(false);

    expect(validateCreditCardExpiryDate(0,21)).toBe(false);
    expect(validateCreditCardExpiryDate('0',21)).toBe(false);
    expect(validateCreditCardExpiryDate(2,0)).toBe(false);
    expect(validateCreditCardExpiryDate(2,'0')).toBe(false);

    expect(validateCreditCardExpiryDate(undefined,21)).toBe(false);
    expect(validateCreditCardExpiryDate(2,undefined)).toBe(false);
    expect(validateCreditCardExpiryDate(null,21)).toBe(false);
    expect(validateCreditCardExpiryDate(2,null)).toBe(false);
    expect(validateCreditCardExpiryDate('',21)).toBe(false);
    expect(validateCreditCardExpiryDate(2,'')).toBe(false);
  });

  it(`should failed when inputted an expired credit card`, () => {
    expect(validateCreditCardExpiryDate(2,11)).toBe(false);
    expect(validateCreditCardExpiryDate(3,18)).toBe(false);
  });

});


describe('PaymentScreen', () => {

  it(`should return a positive number when fetching creditBalance`, async () => {
    // const creditBalance = await getCreditBalance(1);
    // expect(creditBalance).toBeGreaterThanOrEqual(0);
    throw 'not implemented'
  });

  it(`should tell success if voucher code is valid`, () => {
    throw 'not implemented'
  });
  it(`should apology if voucher code is invalid`, () => {
    throw 'not implemented'
  });
  it(`should apology if voucher code expired`, () => {
    throw 'not implemented'
  });
  it(`should failed when checking a voucher code valid for different product`, () => {
    throw 'not implemented'
  });

  //// BANK TRANSFER
  it(`should show instruction and Travorama Bank Account No. when choosing "Bank Transfer"`, () => {
    throw 'not implemented'
  });

  it(`should success when inputted all the valid credit card data`, () => {
    throw 'not implemented'
  });

  it(`should redirect to paymentSuccessScreen after invoking pay`, () => {
    throw 'not implemented'
  });
  it(`should apology when 'pay' got error`, () => {
    throw 'not implemented'
  });
  
});
