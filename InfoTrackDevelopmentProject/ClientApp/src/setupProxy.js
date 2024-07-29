const { createProxyMiddleware } = require('http-proxy-middleware');
const { env } = require('process');

// Determine the target URL based on environment variables
let target;

if (env.ASPNETCORE_HTTPS_PORT) {
    target = `https://localhost:${env.ASPNETCORE_HTTPS_PORT}`;
} else if (env.ASPNETCORE_URLS) {
    target = env.ASPNETCORE_URLS.split(';')[0];
} else {
    target = 'http://localhost:5000';
}

const context = [
    "/api" // Ensure this matches the base path of your API endpoints
];

const onError = (err, req, res) => {
    console.error(`Proxy error: ${err.message}`);
    res.writeHead(500, {
        'Content-Type': 'text/plain'
    });
    res.end('Something went wrong. And we are reporting a custom error message.');
};

module.exports = function (app) {
    app.use(
        createProxyMiddleware(context, {
            target: target,
            changeOrigin: true, // Ensure the origin is changed to the target
            secure: false, // Set to true if using valid certificates
            onError: onError,
            headers: {
                Connection: 'Keep-Alive'
            }
        })
    );
};