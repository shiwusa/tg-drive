export interface DirectoryDto {
  id: number;
  name: string;
  ownerId: number;
  readAccessKey: string | null;
  writeAccessKey: string | null;
  parentId: number | null;
  leaf: boolean;
}

export interface FileDto {
  id: number;
  name: string;
  description: string | null;
  addedByUserId: number;
  messageId: number;
  chatId: number;
  readAccessKey: string | null;
  directoryId: number;
}
