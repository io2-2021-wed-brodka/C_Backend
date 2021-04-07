export const delay = <T>(data: T, error?: Error) =>
  new Promise((resolve, reject) => {
    setTimeout(() => {
      if (error) {
        reject(error);
      } else {
        resolve(data);
      }
    }, 1000);
  }) as Promise<T>;
