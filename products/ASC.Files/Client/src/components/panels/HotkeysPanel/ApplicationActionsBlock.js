import React from "react";
import Row from "@appserver/components/row";
import Text from "@appserver/components/text";

const ApplicationActionsBlock = ({ t, textStyles, keyTextStyles }) => {
  return (
    <>
      <Row className="hotkeys_row">
        <>
          <Text {...textStyles}>{t("HotkeysShortcuts")}</Text>
          <Text {...keyTextStyles}>? {t("HotkeysOr")} Ctrl+/</Text>
        </>
      </Row>
      <Row className="hotkeys_row">
        <>
          <Text {...textStyles}>{t("HotkeysFind")}</Text>
          <Text {...keyTextStyles}>Ctrl+f</Text>
        </>
      </Row>
    </>
  );
};

export default ApplicationActionsBlock;
