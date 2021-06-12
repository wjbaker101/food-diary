const path = require('path');

module.exports = {
    outputDir: path.resolve(__dirname, "../backend/wwwroot"),

    css: {
        loaderOptions: {
            sass: {
                prependData: `@import 'src/style/global.scss';`,
            },
        },
    },

    devServer: {
        proxy: {
            '/api': {
                target: 'https://localhost:44386',
                ws: true,
                changeOrigin: true,
            }
        },
        disableHostCheck: true,
    },
}
