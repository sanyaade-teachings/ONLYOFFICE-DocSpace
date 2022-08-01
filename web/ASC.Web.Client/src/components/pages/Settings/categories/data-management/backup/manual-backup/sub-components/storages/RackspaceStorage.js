import React from "react";
import { withTranslation } from "react-i18next";
import { inject, observer } from "mobx-react";
import Button from "@appserver/components/button";
import RackspaceSettings from "../../../consumer-storage-settings/RackspaceSettings";

class RackspaceStorage extends React.Component {
  constructor(props) {
    super(props);
    const { selectedStorage, setCompletedFormFields } = this.props;

    setCompletedFormFields(RackspaceSettings.formNames());

    this.isDisabled = selectedStorage && !selectedStorage.isSet;
  }

  render() {
    const {
      t,
      isLoadingData,
      isMaxProgress,
      selectedStorage,
      buttonSize,
      onMakeCopyIntoStorage,
      isValidForm,
    } = this.props;

    return (
      <>
        <RackspaceSettings
          isLoadingData={isLoadingData}
          selectedStorage={selectedStorage}
          t={t}
        />

        <div className="manual-backup_buttons">
          <Button
            label={t("Common:Duplicate")}
            onClick={onMakeCopyIntoStorage}
            primary
            isDisabled={!isValidForm || !isMaxProgress || this.isDisabled}
            size={buttonSize}
          />
          {!isMaxProgress && (
            <Button
              label={t("Common:CopyOperation") + "..."}
              isDisabled
              size={buttonSize}
              style={{ marginLeft: "8px" }}
            />
          )}
        </div>
      </>
    );
  }
}

export default inject(({ backup }) => {
  const { setCompletedFormFields, isValidForm } = backup;

  return {
    isValidForm,
    setCompletedFormFields,
  };
})(observer(withTranslation(["Settings", "Common"])(RackspaceStorage)));
