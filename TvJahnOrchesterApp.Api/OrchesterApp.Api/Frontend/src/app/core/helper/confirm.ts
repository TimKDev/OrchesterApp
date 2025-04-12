export function confirmDialog(header: string, message: string) {
  return (target: any, propertyKey: string, descriptor: PropertyDescriptor) => {
    const originalMethod = descriptor.value;
    descriptor.value = async function (...args: unknown[]) {
      let alertController = (this as any).alertController;
      if(!alertController) throw new Error("Dieser Decorator kann nur in Klassen verwendet werden, die den Ionic AlertController als Property unter dem Name 'alertController' gespeichert haben. Bitte injekten Sie den AlertController.");
      let alert = await alertController.create({
        header,
        message,
        buttons: [
          {
            text: 'Abbrechen',
            role: 'cancel',
          },
          {
            text: 'Ok',
            role: 'confirm',
          },
        ]
      });
      await alert.present();
      let alertResult = await alert.onDidDismiss();
      if (alertResult.role === 'confirm') {
        return originalMethod.apply(this, args);
      }
    }
    return descriptor;
  }
}
