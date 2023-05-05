import React, { useEffect, useLayoutEffect, useState, useRef } from "react";

import { loadScript, combineUrl } from "@docspace/common/utils";

import PDFViewerProps from "./PDFViewer.props";
import ViewerLoader from "../ViewerLoader";
import MainPanel from "./ui/MainPanel";
import Sidebar from "./ui/Sidebar";

import {
  ErrorMessage,
  PdfViewrWrapper,
  DesktopTopBar,
  PDFToolbar,
} from "./PDFViewer.styled";

import { ToolbarActionType } from "../../helpers";
import { ToolbarItemType } from "../ImageViewerToolbar/ImageViewerToolbar.props";

import "./lib";

// import { isDesktop } from "react-device-detect";?
const pdfViewerId = "pdf-viewer";

function PDFViewer({
  src,
  title,
  toolbar,
  isPDFSidebarOpen,
  onMask,
  generateContextMenu,
  setIsOpenContextMenu,
  setIsPDFSidebarOpen,
}: PDFViewerProps) {
  const containerRef = useRef<HTMLDivElement>(null);

  const [file, setFile] = useState<ArrayBuffer | string | null>();

  const [isError, setIsError] = useState<boolean>(false);

  const [isLoadedViewerScript, setIsLoadedViewerScript] = useState<boolean>(
    () => {
      const result = document.getElementById(pdfViewerId);
      return result !== null;
    }
  );

  const [isLoadingScript, setIsLoadingScript] = useState<boolean>(false);
  const [isLoadingFile, setIsLoadingFile] = useState<boolean>(false);

  const resize = () => {
    //@ts-ignore
    window.Viewer && window.Viewer.resize();
    //@ts-ignore
    window.Thumbnails && window.Thumbnails.resize();
  };

  useEffect(() => {
    window.addEventListener("resize", resize);

    return () => {
      window.removeEventListener("resize", resize);
    };
  }, []);

  useLayoutEffect(() => {
    const origin = window.location.origin;
    //@ts-ignore
    const path = window.DocSpaceConfig.pdfViewerUrl;

    if (!isLoadedViewerScript) {
      setIsLoadingScript(true);
      loadScript(
        combineUrl(origin, path),
        pdfViewerId,
        () => {
          //@ts-ignore
          window.Viewer = new window.AscViewer.CViewer("mainPanel", {
            theme: { type: "dark" },
          });
          //@ts-ignore
          window.Thumbnails =
            //@ts-ignore
            window.Viewer.createThumbnails("viewer-thumbnail");
          //@ts-ignore
          window.Thumbnails.setZoom(0.2);

          setIsLoadedViewerScript(true);
          setIsLoadingScript(false);
        },
        (event: any) => {
          setIsLoadingScript(false);
          setIsError(true);
          console.error(event);
        }
      );
    }
  }, []);

  useEffect(() => {
    setIsLoadingFile(true);
    fetch(src)
      .then((value) => {
        return value.blob();
      })
      .then((value) => {
        const reader = new FileReader();
        reader.onload = function (e) {
          setFile(e.target?.result);
        };
        reader.readAsArrayBuffer(value);
      })
      .catch((event) => {
        setIsError(true);
        console.error(event);
      })
      .finally(() => {
        setIsLoadingFile(false);
      });
  }, [src]);

  useEffect(() => {
    if (isLoadedViewerScript && !isLoadingFile && file) {
      try {
        if (!containerRef.current?.hasChildNodes()) {
          //@ts-ignore
          window.Viewer = new window.AscViewer.CViewer("mainPanel", {
            theme: { type: "dark" },
          });
          //@ts-ignore
          window.Thumbnails =
            //@ts-ignore
            window.Viewer.createThumbnails("viewer-thumbnail");
        }
        //@ts-ignore
        window.Viewer.open(file);
        resize();
      } catch (error) {
        setIsError(true);
        console.log(error);
      }
    }
  }, [file, isLoadedViewerScript, isLoadingFile]);

  useEffect(() => {
    if (isLoadedViewerScript && containerRef.current?.hasChildNodes()) resize();
  }, [isPDFSidebarOpen, isLoadedViewerScript]);

  function toolbarEvent(item: ToolbarItemType) {
    switch (item.actionType) {
      case ToolbarActionType.Panel:
        setIsPDFSidebarOpen((prev) => !prev);
        break;

      default:
        break;
    }
  }

  if (isError) {
    return (
      <PdfViewrWrapper>
        <ErrorMessage>Something went wrong</ErrorMessage>
      </PdfViewrWrapper>
    );
  }

  return (
    <>
      <DesktopTopBar
        title={title}
        onMaskClick={onMask}
        isPanelOpen={isPDFSidebarOpen}
      />

      <PdfViewrWrapper>
        <ViewerLoader isLoading={isLoadingFile || isLoadingScript} />
        <Sidebar
          isPanelOpen={isPDFSidebarOpen}
          setIsPDFSidebarOpen={setIsPDFSidebarOpen}
        />
        <MainPanel
          ref={containerRef}
          isLoading={isLoadingFile || isLoadingScript}
        />
      </PdfViewrWrapper>

      <PDFToolbar
        toolbar={toolbar}
        percentValue={1}
        isPanelOpen={isPDFSidebarOpen}
        toolbarEvent={toolbarEvent}
        generateContextMenu={generateContextMenu}
        setIsOpenContextMenu={setIsOpenContextMenu}
      />
    </>
  );
}

export default PDFViewer;
