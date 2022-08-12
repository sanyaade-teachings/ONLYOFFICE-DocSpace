import React from "react";

import SelectFileInputWrapper from "client/SelectFolderInput";
class Documents extends React.Component {
  render() {
    const { t } = this.props;
    return (
      <SelectFileInputWrapper
        {...this.props}
        foldersType="rooms"
        withoutProvider
        isArchiveOnly
        searchParam=".gz"
        filesListTitle={t("SelectFileInGZFormat")}
        withoutResetFolderTree
        ignoreSelectedFolderTree
        maxFolderInputWidth="446px"
        withoutBasicSelection
      />
    );
  }
}
export default Documents;
