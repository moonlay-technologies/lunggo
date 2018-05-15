/// <binding BeforeBuild='Run - Development' />
"use strict";
const path = require('path');
var PROD = JSON.parse(process.env.PROD_ENV || '0');

// webpack.config.js
var HardSourceWebpackPlugin = require('hard-source-webpack-plugin');

module.exports = {
    entry: [
        "babel-polyfill",
        "./Assets/js/Payment/PaymentPageStateContainer.jsx"
    ],
    output: {
        path: path.resolve(__dirname, "Assets/js/Payment/"),
        filename: "paymentReact.bundle.js"
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
                        "es2016",
                        "stage-2",
                        "react"
                    ]
                }
            }
        ]
    },
    plugins: [
        new HardSourceWebpackPlugin()
    ]
};
