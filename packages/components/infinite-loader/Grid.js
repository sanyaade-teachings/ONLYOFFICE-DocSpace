import React, { useCallback } from "react";
import { InfiniteLoader, WindowScroller, AutoSizer } from "react-virtualized";
import { StyledList } from "./StyledInfiniteLoader";

const GridComponent = ({
  hasMoreFiles,
  filesLength,
  itemCount,
  loadMoreItems,
  onScroll,
  countTilesInRow,
  children,
  className,
  scroll,
}) => {
  const isItemLoaded = useCallback(
    ({ index }) => {
      return !hasMoreFiles || index * countTilesInRow < filesLength;
    },
    [filesLength, hasMoreFiles, countTilesInRow]
  );

  const renderTile = ({ index, style, key }) => {
    return (
      <div className="window-item" style={style} key={key}>
        {children[index]}
      </div>
    );
  };

  const getItemSize = ({ index }) => {
    const itemClassNames = children[index]?.props?.className;
    const isFile = itemClassNames?.includes("isFile");
    const isFolder = itemClassNames?.includes("isFolder");

    const horizontalGap = 16;
    const verticalGap = 14;
    const headerMargin = 15;

    const folderHeight = 64 + verticalGap;
    const fileHeight = 220 + horizontalGap;
    const titleHeight = 20 + headerMargin;

    return isFolder ? folderHeight : isFile ? fileHeight : titleHeight;
  };

  return (
    <InfiniteLoader
      isRowLoaded={isItemLoaded}
      rowCount={itemCount}
      loadMoreRows={loadMoreItems}
    >
      {({ onRowsRendered, registerChild }) => (
        <WindowScroller scrollElement={scroll}>
          {({ height, isScrolling, onChildScroll, scrollTop }) => (
            <AutoSizer>
              {({ width }) => (
                <StyledList
                  autoHeight
                  height={height}
                  onRowsRendered={onRowsRendered}
                  ref={registerChild}
                  rowCount={children.length}
                  rowHeight={getItemSize}
                  rowRenderer={renderTile}
                  width={width}
                  className={className}
                  isScrolling={isScrolling}
                  onChildScroll={onChildScroll}
                  scrollTop={scrollTop}
                  overscanRowCount={3}
                  onScroll={onScroll}
                />
              )}
            </AutoSizer>
          )}
        </WindowScroller>
      )}
    </InfiniteLoader>
  );
};

export default GridComponent;
