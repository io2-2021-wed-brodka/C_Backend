export const isAdminApp = () => process.env.REACT_APP_FOR === 'admin';

export const isUserApp = () => process.env.REACT_APP_FOR === 'user';
