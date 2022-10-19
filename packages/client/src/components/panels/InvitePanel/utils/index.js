import { ShareAccessRights, RoomsType } from "@docspace/common/constants";

export const getAccessOptions = (
  t,
  roomType = RoomsType.CustomRoom,
  withRemove = false,
  withSeparator = false
) => {
  let options = [];
  const accesses = {
    docSpaceAdmin: {
      key: "docSpaceAdmin",
      label: t("Translations:RoleDocSpaceAdmin"),
      description: t("Translations:RoleDocSpaceAdminDescription"),
      quota: t("Common:Paid"),
      color: "#EDC409",
      access: ShareAccessRights.FullAccess,
    },
    roomAdmin: {
      key: "roomAdmin",
      label: t("Translations:RoleRoomAdmin"),
      description: t("Translations:RoleRoomAdminDescription"),
      quota: t("Common:Paid"),
      color: "#EDC409",
      access: ShareAccessRights.RoomManager,
    },
    editor: {
      key: "editor",
      label: t("Translations:RoleEditor"),
      description: t("Translations:RoleEditorDescription"),
      access: ShareAccessRights.Editing,
    },
    formFiller: {
      key: "formFiller",
      label: t("Translations:RoleFormFiller"),
      description: t("Translations:RoleFormFillerDescription"),
      access: ShareAccessRights.FormFilling,
    },
    reviewer: {
      key: "reviewer",
      label: t("Translations:RoleReviewer"),
      description: t("Translations:RoleReviewerDescription"),
      access: ShareAccessRights.Review,
    },
    commentator: {
      key: "commentator",
      label: t("Translations:RoleCommentator"),
      description: t("Translations:RoleCommentatorDescription"),
      access: ShareAccessRights.Comment,
    },
    viewer: {
      key: "viewer",
      label: t("Translations:RoleViewer"),
      description: t("Translations:RoleViewerDescription"),
      access: ShareAccessRights.ReadOnly,
    },
  };

  switch (roomType) {
    case RoomsType.FillingFormsRoom:
      options = [
        accesses.roomAdmin,
        { key: "s1", isSeparator: withSeparator },
        accesses.formFiller,
        accesses.viewer,
      ];
      break;
    case RoomsType.EditingRoom:
      options = [
        accesses.roomAdmin,
        { key: "s1", isSeparator: withSeparator },
        accesses.editor,
        accesses.viewer,
      ];
      break;
    case RoomsType.ReviewRoom:
      options = [
        accesses.roomAdmin,
        { key: "s1", isSeparator: withSeparator },
        accesses.reviewer,
        accesses.commentator,
        accesses.viewer,
      ];
      break;
    case RoomsType.ReadOnlyRoom:
      options = [
        accesses.roomAdmin,
        { key: "s1", isSeparator: withSeparator },
        accesses.viewer,
      ];
      break;
    case RoomsType.CustomRoom:
      options = [
        accesses.roomAdmin,
        { key: "s1", isSeparator: withSeparator },
        accesses.editor,
        accesses.formFiller,
        accesses.reviewer,
        accesses.commentator,
        accesses.viewer,
      ];
      break;
  }

  const removeOption = [
    {
      key: "s2",
      isSeparator: true,
    },
    {
      key: "remove",
      label: t("Translations:Remove"),
    },
  ];

  return withRemove ? [...options, ...removeOption] : options;
};