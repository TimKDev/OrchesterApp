import { NotificationType } from "../services/notification-type.enum";
import { NotificationUrgency } from "../services/notification-urgency.enum";

export interface NotificationDto {
  id: string;
  title: string;
  message: string;
  isRead: boolean;
  type: NotificationType;
  urgency: NotificationUrgency;
  terminId?: string;
  createdAt: Date;
}