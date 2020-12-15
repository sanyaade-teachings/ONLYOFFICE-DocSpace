import React from "react";
import styled from "styled-components";
import equal from "fast-deep-equal/react";
import { tablet } from "@appserver/components/src/utils/device";

const StyledSectionHeader = styled.div`
  border-bottom: 1px solid #eceef1;
  height: 55px;
  margin-right: 24px;
  margin-top: -1px;

  @media ${tablet} {
    margin-right: 16px;
    border-bottom: none;
    ${(props) =>
      props.borderBottom &&
      `
      border-bottom: 1px solid #eceef1;
      padding-bottom: 16px
    `};
    height: 49px;
  }

  .section-header {
    @media ${tablet} {
      width: 100%;
      padding-top: 4px;
    }
      }
`;

class SectionHeader extends React.Component {
  shouldComponentUpdate(nextProps) {
    return !equal(this.props, nextProps);
  }

  render() {
    //console.log("PageLayout SectionHeader render");
    // eslint-disable-next-line react/prop-types
    const { borderBottom, ...rest } = this.props;

    return (
      <StyledSectionHeader
        borderBottom={borderBottom}
      >
        <div className="section-header" {...rest} />
      </StyledSectionHeader>
    );
  }
}

SectionHeader.displayName = "SectionHeader";

export default SectionHeader;
