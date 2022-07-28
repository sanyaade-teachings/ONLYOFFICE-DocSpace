import React from "react";
import { inject, observer } from "mobx-react";
import IconButton from "@docspace/components/icon-button";
import { withTranslation } from "react-i18next";
import { withRouter } from "react-router-dom";
import { AppServerConfig } from "@docspace/common/constants";
import {
  StyledHeadline,
  StyledContainer,
  StyledInfoPanelToggleWrapper,
} from "./StyledGallery";
import config from "PACKAGE_FILE";
import FilesFilter from "@docspace/common/api/files/filter";
import { combineUrl } from "@docspace/common/utils";

const SectionHeaderContent = (props) => {
  const {
    t,
    history,
    match,
    isInfoPanelVisible,
    toggleInfoPanel,
    setGallerySelected,
  } = props;

  const onBackToFiles = () => {
    setGallerySelected(null);
    const filter = FilesFilter.getDefault();
    filter.folder = match.params.folderId;
    const urlFilter = filter.toUrlParams();

    history.push(
      combineUrl(
        AppServerConfig.proxyURL,
        config.homepage,
        `/filter?${urlFilter}`
      )
    );
  };

  return (
    <StyledContainer>
      <IconButton
        iconName="/static/images/arrow.path.react.svg"
        size="17"
        isFill
        onClick={onBackToFiles}
        className="arrow-button"
      />

      <StyledHeadline type="content" truncate>
        {t("Common:OFORMsGallery")}
      </StyledHeadline>
      <StyledInfoPanelToggleWrapper isInfoPanelVisible={isInfoPanelVisible}>
        <div className="info-panel-toggle-bg">
          <IconButton
            className="info-panel-toggle"
            iconName="images/panel.react.svg"
            size="16"
            isFill={true}
            onClick={toggleInfoPanel}
          />
        </div>
      </StyledInfoPanelToggleWrapper>
    </StyledContainer>
  );
};

export default inject(({ auth, filesStore }) => {
  const { toggleIsVisible, isVisible } = auth.infoPanelStore;
  const { setGallerySelected } = filesStore;
  return {
    toggleInfoPanel: toggleIsVisible,
    isInfoPanelVisible: isVisible,
    setGallerySelected,
  };
})(withTranslation("Common")(withRouter(observer(SectionHeaderContent))));
