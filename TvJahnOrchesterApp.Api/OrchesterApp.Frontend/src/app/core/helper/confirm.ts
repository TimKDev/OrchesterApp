import { AlertController } from "@ionic/angular";

export function confirmDialog (header: string, message: string) {
  return (target: any, propertyKey: string, descriptor: PropertyDescriptor) => {
    const originalMethod = descriptor.value;
    descriptor.value = async (...args: unknown[]) => {
      debugger;
      let alert = await this.alertController.create({
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
      if(alertResult.role === 'confirm'){
        const result = originalMethod.apply(this, args);
        return result;
      }
    }
    return descriptor;

  }
}
