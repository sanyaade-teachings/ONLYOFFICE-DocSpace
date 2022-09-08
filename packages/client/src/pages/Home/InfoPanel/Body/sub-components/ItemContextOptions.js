import React from "react";
import styled from "styled-components";
import { inject } from "mobx-react";
import { withTranslation } from "react-i18next";

import { ContextMenuButton } from "@docspace/components";

import ContextHelper from "../helpers/ContextHelper";

const StyledItemContextOptions = styled.div`
  margin-left: auto;
`;

const ItemContextOptions = (props) => {
  if (!props.selection) return null;

  const contextHelper = new ContextHelper(props);
  return (
    <StyledItemContextOptions>
      <ContextMenuButton
        zIndex={402}
        className="option-button"
        directionX="right"
        iconName="images/vertical-dots.react.svg"
        size={15}
        isFill
        getData={contextHelper.getItemContextOptions}
        isDisabled={false}
      />
    </StyledItemContextOptions>
  );
};

export default inject(({ filesStore, contextOptionsStore }) => {
  return {
    getContextOptions: filesStore.getFilesContextOptions,
    getContextOptionActions: contextOptionsStore.getFilesContextOptions,
  };
})(
  withTranslation([
    "Files",
    "Common",
    "Translations",
    "InfoPanel",
    "SharingPanel",
  ])(ItemContextOptions)
);
