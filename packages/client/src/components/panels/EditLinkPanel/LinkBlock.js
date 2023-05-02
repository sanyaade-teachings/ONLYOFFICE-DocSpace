import React, { useState, startTransition } from "react";
import Text from "@docspace/components/text";
import Link from "@docspace/components/link";
import TextInput from "@docspace/components/text-input";

const LinkBlock = (props) => {
  const { t } = props;

  const [linkNameValue, setLinkNameValue] = useState("");
  const [linkValue, setLinkValue] = useState("");

  const onChangeLinkName = (e) => {
    startTransition(() => {
      setLinkNameValue(e.target.value);
    });
  };

  const onChangeLink = (e) => {
    // setLinkValue(e.target.value);
  };

  const onShortenClick = () => {
    alert("onShortenClick");
  };

  return (
    <div className="edit-link_link-block">
      <Text className="edit-link-text" fontSize="13px" fontWeight={600}>
        {t("LinkName")}
      </Text>
      <TextInput
        scale
        size="base"
        withBorder
        isAutoFocussed
        className="edit-link_name-input"
        value={linkNameValue}
        onChange={onChangeLinkName}
        placeholder={t("ExternalLink")}
      />

      <TextInput
        scale
        size="base"
        withBorder
        isDisabled
        className="edit-link_link-input"
        value={linkValue}
        onChange={onChangeLink}
        placeholder={t("ExternalLink")}
      />

      <Link
        fontSize="13px"
        fontWeight={600}
        isHovered
        type="action"
        onClick={onShortenClick}
      >
        {t("Shorten")}
      </Link>
    </div>
  );
};

export default LinkBlock;
