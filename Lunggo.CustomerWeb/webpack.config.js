"use strict";
const path = require('path');
var PROD = JSON.parse(process.env.PROD_ENV || '0');

module.exports = {
    entry: [
        "babel-polyfill",
        "./Assets/js/Payment/PaymentPageStateContainer.jsx"
    ],
    output: {
        path: path.resolve(__dirname, "Assets/js/Payment/"),
        filename: "paymentReact.js"
    },
    devServer: {
        contentBase: ".",
        host: "localhost",
        port: 9000
    },
    module: {
        rules: [
            {
                test: /\.jsx?$/,
                loader: "babel-loader",
                options: {
                    presets: [
                        "es2015",
                        "es2016",
                        "stage-0",
                        "stage-1",
                        "stage-2",
                        "react"
                    ]
                }
            }
        ]
    }
};
