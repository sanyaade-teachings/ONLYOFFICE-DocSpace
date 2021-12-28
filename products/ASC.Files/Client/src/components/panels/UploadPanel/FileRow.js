import React, { Component } from "react";
import styled, { css } from "styled-components";
import Row from "@appserver/components/row";
import Text from "@appserver/components/text";
import Link from "@appserver/components/link";
import LoadingButton from "./LoadingButton";
import { inject, observer } from "mobx-react";
import { withTranslation } from "react-i18next";
import SimulatePassword from "./SimulatePassword";
import ErrorFileUpload from "./ErrorFileUpload.js";
import ActionsUploadedFile from "./ActionsUploadedFile";
import { isMobile } from "react-device-detect";
import NoUserSelect from "@appserver/components/utils/commonStyles";

const StyledFileRow = styled(Row)`
  /* margin: 0 16px; */
  width: calc(100% - 16px);
  box-sizing: border-box;
  /* font-weight: 600; */
  /* padding-right: 16px; */
  padding-left: 16px;
  max-width: 484px;

  ::after {
    max-width: 468px;
    width: calc(100% - 16px);
  }

  ${!isMobile && "min-height: 40px;"}

  height: 100%;

  .styled-element,
  .row_content {
    ${(props) =>
      props.showPasswordInput &&
      css`
        margin-top: ${isMobile ? "-44px" : "-48px"};
      `}
  }

  .upload-panel_file-name {
    max-width: 412px;
    overflow: hidden;
    text-overflow: ellipsis;
    align-items: center;
    display: flex;
  }

  .enter-password {
    white-space: nowrap;
    max-width: 97px;
    overflow: hidden;
    ${NoUserSelect}
  }
  .password-input {
    position: absolute;
    top: 44px;
    left: 16px;
    max-width: 470px;
    width: calc(100% - 16px);
    .conversation-password-wrapper {
      max-width: 470px;
      width: 100%;
    }
  }

  .row_content > a,
  .row_content > p {
    margin: auto 0;
    line-height: 16px;
  }

  .row_context-menu-wrapper {
    display: none;
  }
  .upload_panel-icon {
    margin-left: auto;
    padding-left: 16px;
    line-height: 24px;
    display: flex;
    align-items: center;
    flex-direction: row-reverse;

    svg {
      width: 16px;
      height: 16px;
    }

    .enter-password {
      margin-right: 8px;
      text-decoration: underline dashed;
      cursor: pointer;
    }
  }

  .img_error {
    filter: grayscale(1);
  }

  .convert_icon {
    padding-right: 12px;
  }

  .__react_component_tooltip.type-light {
    background-color: #f8f7bf !important;
    box-shadow: none;
    -moz-box-shadow: none;
    -webkit-box-shadow: none;
  }
  .__react_component_tooltip.place-left::after {
    border-left: 6px solid #f8f7bf !important;
  }

  .__react_component_tooltip.place-right::after {
    border-right: 6px solid #f8f7bf !important;
  }

  .__react_component_tooltip.place-top::after {
    border-top: 6px solid #f8f7bf !important;
  }

  .__react_component_tooltip.place-bottom::after {
    border-bottom: 6px solid #f8f7bf !important;
  }

  .upload-panel_file-row-link {
    ${(props) =>
      !props.isMediaActive &&
      css`
        cursor: default;
      `}
  }
`;
class FileRow extends Component {
  constructor(props) {
    super(props);

    this.state = {
      showPasswordInput: false,
    };
  }

  onTextClick = () => {
    const { showPasswordInput } = this.state;
    const { updateRowsHeight, index } = this.props;

    const newState = !showPasswordInput;

    this.setState({ showPasswordInput: newState }, () => {
      updateRowsHeight && updateRowsHeight(index, newState);
    });
  };

  onCancelCurrentUpload = (e) => {
    //console.log("cancel upload ", e);
    const { id, action, fileId } = e.currentTarget.dataset;
    const { cancelCurrentUpload, cancelCurrentFileConversion } = this.props;

    return action === "convert"
      ? cancelCurrentFileConversion(fileId)
      : cancelCurrentUpload(id);
  };

  onMediaClick = (id) => {
    const { setMediaViewerData, setUploadPanelVisible } = this.props;
    if (!isMediaActive) return;
    const item = { visible: true, id: id };
    setMediaViewerData(item);
    setUploadPanelVisible(false);
  };

  render() {
    const {
      t,
      item,
      uploaded,
      //onMediaClick,
      currentFileUploadProgress,
      fileIcon,
      isMedia,
      ext,
      name,
      isPersonal,
      isMediaActive,
      downloadInCurrentTab,
    } = this.props;
    const { showPasswordInput } = this.state;

    const fileExtension = ext ? (
      <Text as="span" fontWeight="600" color="#A3A9AE">
        {ext}
      </Text>
    ) : (
      <></>
    );

    return (
      <>
        <StyledFileRow
          className="download-row"
          key={item.uniqueId}
          checkbox={false}
          element={
            <img className={item.error && "img_error"} src={fileIcon} alt="" />
          }
          isMediaActive={isMediaActive}
          showPasswordInput={showPasswordInput}
        >
          <>
            {item.fileId ? (
              isMedia ? (
                <Link
                  className="upload-panel_file-row-link"
                  fontWeight="600"
                  color={item.error || !isMediaActive ? "#A3A9AE" : ""}
                  truncate
                  onClick={() => this.onMediaClick(item.fileId)}
                >
                  {name}
                </Link>
              ) : (
                <div className="upload-panel_file-name">
                  <Link
                    fontWeight="600"
                    color={item.error && "#A3A9AE"}
                    truncate
                    href={item.fileInfo ? item.fileInfo.webUrl : ""}
                    target={downloadInCurrentTab ? "_self" : "_blank"}
                  >
                    {name}
                    {fileExtension}
                  </Link>
                </div>
              )
            ) : (
              <div className="upload-panel_file-name">
                <Text fontWeight="600" color={item.error && "#A3A9AE"} truncate>
                  {name}
                  {fileExtension}
                </Text>
              </div>
            )}

            {item.fileId && !item.error ? (
              <ActionsUploadedFile
                item={item}
                isPersonal={isPersonal}
                onCancelCurrentUpload={this.onCancelCurrentUpload}
              />
            ) : item.error || (!item.fileId && uploaded) ? (
              <ErrorFileUpload
                t={t}
                item={item}
                onTextClick={this.onTextClick}
                showPasswordInput={showPasswordInput}
              />
            ) : (
              <div
                className="upload_panel-icon"
                data-id={item.uniqueId}
                onClick={this.onCancelCurrentUpload}
              >
                <LoadingButton percent={currentFileUploadProgress} />
              </div>
            )}
            {showPasswordInput && (
              <div className="password-input">
                <SimulatePassword item={item} hideInput={this.onTextClick} />
              </div>
            )}
          </>
        </StyledFileRow>
      </>
    );
  }
}
export default inject(
  ({ auth, formatsStore, uploadDataStore, mediaViewerDataStore }, { item }) => {
    let ext;
    let name;
    let splitted;
    if (item.file) {
      splitted = item.file.name.split(".");
      ext = splitted.length > 1 ? "." + splitted.pop() : "";
      name = splitted[0];
    } else {
      ext = item.fileInfo.fileExst;
      splitted = item.fileInfo.title.split(".");
      name = splitted[0];
    }
    const { personal } = auth.settingsStore;
    const {
      iconFormatsStore,
      mediaViewersFormatsStore,
      docserviceStore,
    } = formatsStore;
    const { canViewedDocs } = docserviceStore;
    const {
      uploaded,
      primaryProgressDataStore,
      cancelCurrentUpload,
      cancelCurrentFileConversion,
      setUploadPanelVisible,
    } = uploadDataStore;
    const { playlist, setMediaViewerData } = mediaViewerDataStore;
    const { loadingFile: file } = primaryProgressDataStore;
    const isMedia = mediaViewersFormatsStore.isMediaOrImage(ext);
    const isMediaActive =
      playlist.findIndex((el) => el.fileId === item.fileId) !== -1;

    const fileIcon = iconFormatsStore.getIconSrc(ext, 24);

    const loadingFile = !file || !file.uniqueId ? null : file;

    const currentFileUploadProgress =
      file && loadingFile.uniqueId === item.uniqueId
        ? loadingFile.percent
        : null;

    const { isArchive } = iconFormatsStore;

    const downloadInCurrentTab = isArchive(ext) || !canViewedDocs(ext);

    return {
      isPersonal: personal,
      currentFileUploadProgress,
      uploaded,
      isMedia,
      fileIcon,
      ext,
      name,
      loadingFile,
      isMediaActive,
      downloadInCurrentTab,

      cancelCurrentUpload,
      cancelCurrentFileConversion,
      setMediaViewerData,
      setUploadPanelVisible,
    };
  }
)(withTranslation("UploadPanel")(observer(FileRow)));
