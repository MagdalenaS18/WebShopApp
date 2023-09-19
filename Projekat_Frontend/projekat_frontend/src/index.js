import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import { GoogleOAuthProvider } from '@react-oauth/google';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(<GoogleOAuthProvider clientId="797764021036-5dh9v25ku57engd46em2ranpjopo2o0c.apps.googleusercontent.com">
<React.StrictMode>
    <App />
</React.StrictMode>
</GoogleOAuthProvider>);


