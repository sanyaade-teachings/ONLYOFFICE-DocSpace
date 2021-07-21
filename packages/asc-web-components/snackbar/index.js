import React from "react";
import ReactDOM from "react-dom";
import PropType from "prop-types";
import StyledSnackBar from "./styled-snackbar";
import StyledCrossIcon from "./styled-snackbar-action";
import StyledLogoIcon from "./styled-snackbar-logo";
import Box from "../box";
import Heading from "../heading";
import Text from "../text";

class SnackBar extends React.Component {
  static show(barConfig) {
    const { parentElementId, ...rest } = barConfig;

    let parentElementNode =
      parentElementId && document.getElementById(parentElementId);

    if (!parentElementNode) {
      const snackbarNode = document.createElement("div");
      snackbarNode.id = "snackbar";
      document.body.appendChild(snackbarNode);
      parentElementNode = snackbarNode;
    }

    window.snackbar = barConfig;

    ReactDOM.render(<SnackBar {...rest} />, parentElementNode);
  }

  static close() {
    if (window.snackbar && window.snackbar.parentElementId) {
      ReactDOM.unmountComponentAtNode(window.snackbar.parentElementId);
    } else {
      console.error("Not found snackbar");
    }
  }

  onActionClick = (e) => {
    this.props.onAction && this.props.onAction(e);
  };

  render() {
    const {
      text,
      headerText,
      btnText,
      textColor,
      showIcon,
      fontSize,
      fontWeight,
      textAlign,
      headerAlign,
      htmlContent,
      ...rest
    } = this.props;

    const headerStyles = headerText ? {} : { display: "none" };

    return (
      <StyledSnackBar {...rest}>
        {htmlContent ? (
          <div
            dangerouslySetInnerHTML={{
              __html: htmlContent,
            }}
          />
        ) : (
          <>
            {showIcon && (
              <Box className="logo">
                <StyledLogoIcon size="medium" color={textColor} />
              </Box>
            )}
            <Box className="text-container">
              <Heading
                size="xsmall"
                isInline={true}
                className="text-header"
                style={headerStyles}
                color={textColor}
                textAlign={headerAlign}
              >
                {headerText}
              </Heading>
              <Text
                as="p"
                color={textColor}
                fontSize={fontSize}
                fontWeight={fontWeight}
                textAlign={textAlign}
              >
                {text}
              </Text>
            </Box>
          </>
        )}
        <button className="action" onClick={this.onActionClick}>
          {btnText ? (
            <Text color={textColor}>{btnText}</Text>
          ) : (
            <StyledCrossIcon size="medium" />
          )}
        </button>
        )
      </StyledSnackBar>
    );
  }
}

SnackBar.propTypes = {
  text: PropType.string,
  headerText: PropType.string,
  btnText: PropType.string,
  backgroundImg: PropType.string,
  backgroundColor: PropType.string,
  textColor: PropType.string,
  showIcon: PropType.bool,
  onAction: PropType.func,
  fontSize: PropType.string,
  fontWeight: PropType.string,
  textAlign: PropType.string,
  headerAlign: PropType.string,
  htmlContent: PropType.string,
};

SnackBar.defaultProps = {
  backgroundColor: "#f8f7bf",
  textColor: "#000",
  showIcon: true,
  fontSize: "13px",
  fontWeight: "400",
  textAlign: "left",
  headerAlign: "left",
  htmlContent: "",
};

export default SnackBar;
