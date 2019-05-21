import { jsx } from '@emotion/core';
import 'react';
import styled, { css } from 'styled-components';
import PropTypes from 'prop-types';

function _extends() {
  _extends = Object.assign || function (target) {
    for (var i = 1; i < arguments.length; i++) {
      var source = arguments[i];

      for (var key in source) {
        if (Object.prototype.hasOwnProperty.call(source, key)) {
          target[key] = source[key];
        }
      }
    }

    return target;
  };

  return _extends.apply(this, arguments);
}

function _taggedTemplateLiteral(strings, raw) {
  if (!raw) {
    raw = strings.slice(0);
  }

  return Object.freeze(Object.defineProperties(strings, {
    raw: {
      value: Object.freeze(raw)
    }
  }));
}

var _jsxFileName = "D:\\GitHub\\CommunityServer-AspNetCore\\web\\ASC.Web.Components\\src\\components\\Button\\index.js";
function _templateObject7() {
  var data = _taggedTemplateLiteral(["\n    &:hover {\n      ", "\n    }"]);
  _templateObject7 = function _templateObject7() {
    return data;
  };
  return data;
}
function _templateObject6() {
  var data = _taggedTemplateLiteral(["\n    &:active {\n      ", "\n    }"]);
  _templateObject6 = function _templateObject6() {
    return data;
  };
  return data;
}
function _templateObject5() {
  var data = _taggedTemplateLiteral(["\n      border-width: 1px;\n      border-style: solid;\n      border-color: ", ";\n    "]);
  _templateObject5 = function _templateObject5() {
    return data;
  };
  return data;
}
function _templateObject4() {
  var data = _taggedTemplateLiteral(["\n  height: ", ";\n\n  line-height: ", ";\n\n  font-size: ", ";\n\n  color: ", ";\n\n  background-color: ", ";\n\n  padding: ", ";\n\n  cursor: ", ";\n\n  font-family: 'Open Sans', sans-serif;\n  border: none;\n  margin: 0;\n  display: inline-block;\n  font-weight: normal;\n  text-align: center;\n  text-decoration: none;\n  vertical-align: middle;\n  border-radius: 3px;\n  -moz-border-radius: 3px;\n  -webkit-border-radius: 3px;\n  touch-callout: none;\n  -o-touch-callout: none;\n  -moz-touch-callout: none;\n  -webkit-touch-callout: none;\n  user-select: none;\n  -o-user-select: none;\n  -moz-user-select: none;\n  -webkit-user-select: none;\n  stroke: none;\n\n  ", "\n  \n  ", "\n\n  ", "\n\n  &:focus {\n    outline: none\n  }\n"]);
  _templateObject4 = function _templateObject4() {
    return data;
  };
  return data;
}
function _templateObject3() {
  var data = _taggedTemplateLiteral(["\n  background-color: ", ";\n  color: ", ";\n"]);
  _templateObject3 = function _templateObject3() {
    return data;
  };
  return data;
}
function _templateObject2() {
  var data = _taggedTemplateLiteral(["\n      border-width: 1px;\n      border-style: solid;\n      border-color: #dadada;\n    "]);
  _templateObject2 = function _templateObject2() {
    return data;
  };
  return data;
}
function _templateObject() {
  var data = _taggedTemplateLiteral(["\n  background-color: ", ";\n  color: #ffffff;\n\n  ", "\n\n"]);
  _templateObject = function _templateObject() {
    return data;
  };
  return data;
}
var activatedCss = css(_templateObject(), function (props) {
  return props.primary ? '#1f97ca' : '#e2e2e2';
}, function (props) {
  return !props.primary && css(_templateObject2());
});
var hoveredCss = css(_templateObject3(), function (props) {
  return props.primary ? '#3db8ec' : '#f5f5f5';
}, function (props) {
  return props.primary ? '#ffffff' : '#666666';
});
var StyledButton = styled.button.attrs(function (props) {
  return {
    disabled: props.isDisabled ? 'disabled' : ''
  };
})(_templateObject4(), function (props) {
  return props.size === 'huge' && '40px' || props.size === 'big' && '32px' || props.size === 'middle' && '24px' || props.size === 'base' && '21px';
}, function (props) {
  return props.size === 'huge' && '15px' || props.size === 'big' && '17px' || '13px';
}, function (props) {
  return (props.size === 'huge' || props.size === 'big') && '15px' || (props.size === 'middle' || props.size === 'base') && '12px';
}, function (props) {
  return props.primary && '#ffffff' || (!props.isDisabled ? '#666666' : '#999');
}, function (props) {
  return !props.isDisabled ? props.primary ? '#2da7db' : '#ebebeb' : props.primary ? '#a6dcf2' : '#f7f7f7';
}, function (props) {
  return props.size === 'huge' && (props.primary ? '12px 30px 13px' : '11px 30px 12px') || props.size === 'big' && (props.primary ? '7px 30px 8px' : '6px 30px 7px') || props.size === 'middle' && (props.primary ? '5px 24px 6px' : '4px 24px 5px') || props.size === 'base' && (props.primary ? '4px 13px' : '3px 12px');
}, function (props) {
  return props.isDisabled ? 'default !important' : 'pointer';
}, function (props) {
  return !props.primary && css(_templateObject5(), function (props) {
    return !props.isDisabled ? '#c4c4c4' : '#ebebeb';
  });
}, function (props) {
  return !props.isDisabled && (props.isActivated ? "".concat(activatedCss) : css(_templateObject6(), activatedCss));
}, function (props) {
  return !props.isDisabled && (props.isHovered ? "".concat(hoveredCss) : css(_templateObject7(), hoveredCss));
});
var Button = function Button(props) {
  return jsx(StyledButton, _extends({}, props, {
    __source: {
      fileName: _jsxFileName,
      lineNumber: 101
    },
    __self: this
  }));
};
Button.propTypes = {
  size: PropTypes.oneOf(['base', 'middle', 'big', 'huge']),
  primary: PropTypes.bool,
  isActivated: PropTypes.bool,
  isHovered: PropTypes.bool,
  isDisabled: PropTypes.bool,
  onClick: PropTypes.func.isRequired
};
Button.defaultProps = {
  primary: false,
  isActivated: false,
  isHovered: false,
  isDisabled: false,
  size: 'base'
};

var _jsxFileName$1 = "D:\\GitHub\\CommunityServer-AspNetCore\\web\\ASC.Web.Components\\src\\components\\TextInput\\index.js";
function _templateObject$1() {
  var data = _taggedTemplateLiteral(["\n  -webkit-appearance: none;\n  border-radius: 3px;\n  box-shadow: none;\n  box-sizing: border-box;\n  border: solid 1px #c7c7c7;\n  -moz-border-radius: 3px;\n  -webkit-border-radius: 3px;\n  background-color: #fff;\n  color: #434341;\n  display: flex;\n  font-family: 'Open Sans', sans-serif;\n  font-size: 18px;  \n  flex: 1 1 0%;\n  outline: none;\n  overflow: hidden;\n  padding: 8px 20px;\n  transition: all 0.2s ease 0s;\n  width: ", ";\n\n    ::-webkit-input-placeholder {\n        color: #b2b2b2;\n        font-family: 'Open Sans',sans-serif\n    }\n\n    :-moz-placeholder {\n        color: #b2b2b2;\n        font-family: 'Open Sans',sans-serif\n    }\n\n    ::-moz-placeholder {\n        color: #b2b2b2;\n        font-family: 'Open Sans',sans-serif\n    }\n\n    :-ms-input-placeholder {\n        color: #b2b2b2;\n        font-family: 'Open Sans',sans-serif\n    }\n\n"]);
  _templateObject$1 = function _templateObject() {
    return data;
  };
  return data;
}
var StyledInput = styled.input.attrs(function (props) {
  return {
    id: props.id,
    name: props.name,
    type: "text",
    value: props.value,
    placeholder: props.placeholder,
    maxLength: props.maxLength,
    onChange: props.onChange,
    onBlur: props.onBlur,
    onFocus: props.onFocus,
    disabled: props.isDisabled,
    readOnly: props.isReadOnly,
    autoFocus: props.isAutoFocussed,
    autoComplete: props.autoComplete,
    tabIndex: props.tabIndex
  };
})(_templateObject$1(), function (props) {
  return props.size === 'base' ? '100px' : props.size === 'middle' ? '200px' : props.size === 'big' ? '358px' : props.size === 'huge' ? '500px' : '100%';
});
var TextInput = function TextInput(props) {
  return jsx(StyledInput, _extends({}, props, {
    __source: {
      fileName: _jsxFileName$1,
      lineNumber: 74
    },
    __self: this
  }));
};
TextInput.propTypes = {
  id: PropTypes.string,
  name: PropTypes.string,
  value: PropTypes.string.isRequired,
  maxLength: PropTypes.number,
  placeholder: PropTypes.string,
  tabIndex: PropTypes.number,
  size: PropTypes.oneOf(['base', 'middle', 'big', 'huge', 'scale']),
  onChange: PropTypes.func,
  onBlur: PropTypes.func,
  onFocus: PropTypes.func,
  isAutoFocussed: PropTypes.bool,
  isDisabled: PropTypes.bool,
  isReadOnly: PropTypes.bool,
  hasError: PropTypes.bool,
  hasWarning: PropTypes.bool,
  autoComplete: PropTypes.string
};

export { Button, TextInput };
