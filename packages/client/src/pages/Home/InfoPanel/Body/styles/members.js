import styled from "styled-components";

import { Base } from "@docspace/components/themes";

const StyledUserTypeHeader = styled.div`
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 8px 0 12px;

  .title {
    font-weight: 600;
    font-size: 13px;
    line-height: 20px;
    color: ${(props) => props.theme.infoPanel.members.subtitleColor};
  }

  .icon {
    path,
    rect {
      fill: ${(props) => props.theme.infoPanel.members.iconColor};
    }
  }
`;

const StyledUserList = styled.div`
  display: flex;
  flex-direction: column;
`;

const StyledUser = styled.div`
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 8px 0;

  .avatar {
    opacity: ${(props) => (props.isExpect ? 0.5 : 1)};
    min-width: 32px;
    min-height: 32px;
  }

  .name {
    opacity: ${(props) => (props.isExpect ? 0.5 : 1)};
    font-weight: 600;
    font-size: 14px;

    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
  }

  .me-label {
    font-weight: 600;
    font-size: 14px;
    color: ${(props) => props.theme.infoPanel.members.meLabelColor};
    margin-left: -8px;
  }

  .role-wrapper {
    padding-left: 8px;
    margin-left: auto;

    font-weight: 600;
    font-size: 13px;
    line-height: 20px;
    white-space: nowrap;

    .disabled-role-combobox {
      color: ${(props) =>
        props.theme.infoPanel.members.disabledRoleSelectorColor};
    }

    .role-combobox {
      .combo-button-label {
        color: ${(props) => props.theme.infoPanel.members.roleSelectorColor};
      }
      .combo-buttons_arrow-icon {
        path {
          fill: ${(props) =>
            props.theme.infoPanel.members.roleSelectorArrowColor};
        }
      }
    }
  }
`;

StyledUserTypeHeader.defaultProps = { theme: Base };

export { StyledUserTypeHeader, StyledUserList, StyledUser };