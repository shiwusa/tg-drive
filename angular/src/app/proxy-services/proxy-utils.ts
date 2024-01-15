const baseApiUrl = 'https://localhost:5050/api';
export const createControllerUrlCreator = (controllerName: string) => (methodName: string) => baseApiUrl + '/' + controllerName + '/' + methodName;
