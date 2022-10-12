import * as React from "react";
import styled from "styled-components";
import Icon, { ActionType } from "./icon";

const ToolbarItem = styled.li`
  height: 48px;
  width: 48px;
  display: flex;
  justify-content: center;
  align-items: center;

  &:hover {
    cursor: pointer;
    ${(props) =>
      props.noHover ? "background-color: none;" : "background-color: #4d4d4d;"}
  }

  svg {
    path,
    rect {
      fill: #fff;
    }
  }
`;

export const defaultToolbars = [
  {
    key: "zoomIn",
    actionType: ActionType.zoomIn,
  },
  {
    key: "zoomOut",
    actionType: ActionType.zoomOut,
  },
  {
    key: "prev",
    actionType: ActionType.prev,
    noHover: true,
  },
  {
    key: "reset",
    actionType: ActionType.reset,
  },
  {
    key: "next",
    actionType: ActionType.next,
    noHover: true,
  },
  {
    key: "rotateLeft",
    actionType: ActionType.rotateLeft,
  },
  {
    key: "rotateRight",
    actionType: ActionType.rotateRight,
  },
  {
    key: "scaleX",
    actionType: ActionType.scaleX,
  },
  {
    key: "scaleY",
    actionType: ActionType.scaleY,
  },
  {
    key: "download",
    actionType: ActionType.download,
  },
];

function deleteToolbarFromKey(toolbars, keys) {
  const targetToolbar = toolbars.filter((item) => keys.indexOf(item.key) < 0);

  return targetToolbar;
}

export default function ViewerToolbar(props) {
  function handleAction(config) {
    props.onAction(config);
  }

  function renderAction(config) {
    let content = null;
    if (typeof ActionType[config.actionType] !== "undefined") {
      content = <Icon type={config.actionType} />;
    }
    if (config.render) {
      content = config.render;
    }

    return (
      <ToolbarItem
        noHover={config.noHover}
        key={config.key}
        className={`${props.prefixCls}-btn`}
        onClick={() => {
          handleAction(config);
        }}
        data-key={config.key}
      >
        {content}
      </ToolbarItem>
    );
  }
  let attributeNode = props.attribute ? (
    <p className={`${props.prefixCls}-attribute`}>
      {props.alt && `${props.alt}`}
      {props.noImgDetails || (
        <span className={`${props.prefixCls}-img-details`}>
          {`(${props.width} x ${props.height})`}
        </span>
      )}
      {props.showTotal && (
        <span className={`${props.prefixCls}-showTotal`}>
          {`${props.activeIndex + 1} ${props.totalName} ${props.count}`}
        </span>
      )}
    </p>
  ) : null;
  let toolbars = props.toolbars;
  if (!props.zoomable) {
    toolbars = deleteToolbarFromKey(toolbars, ["zoomIn", "zoomOut"]);
  }
  if (!props.changeable) {
    toolbars = deleteToolbarFromKey(toolbars, ["prev", "next"]);
  }
  if (!props.rotatable) {
    toolbars = deleteToolbarFromKey(toolbars, ["rotateLeft", "rotateRight"]);
  }
  if (!props.scalable) {
    toolbars = deleteToolbarFromKey(toolbars, ["scaleX", "scaleY"]);
  }
  if (!props.downloadable) {
    toolbars = deleteToolbarFromKey(toolbars, ["download"]);
  }

  return (
    <div>
      {/* {attributeNode} */}
      <ul className={`${props.prefixCls}-toolbar`}>
        {toolbars.map((item) => {
          return renderAction(item);
        })}
      </ul>
    </div>
  );
}
