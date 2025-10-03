export interface SendCustomNotificationRequest {
  title: string;
  message: string;
  shouldEmailBeSend: boolean;
  orchestermitgliedIds: string[];
}

