<template>
  <div class="download_box" @click="download()">
    <slot></slot>
  </div>
</template>

<script>
export default {
  props: {
    id: {
      required: true,
    },
  },
  data() {
    return {
      load: false,
      fileName: null,
      fileFormat: null,
      fileBase64: null,
    };
  },
  methods: {
    async downloadFile() {
      if (!this.id) return;
      try {
        const respons = await this.axios.get(`File?id=${this.id}`);
        if (respons.data.dataBase64) this.fileBase64 = respons.data.dataBase64;
        if (respons.data.info.name) {
          const nameArray = respons.data.info.name.split(".");
          this.fileName = nameArray[0] ?? "";
          this.fileFormat = nameArray[nameArray.length - 1];
        }
      } catch (error) {
        alert("Operation invalid");
        return;
      }
      this.load = true;
    },
    async download() {
      if (!this.load) await this.downloadFile();

      const blob = new Blob([this.fileBase64], {
        type: this.getMimeType(this.fileFormat),
      });

      const link = document.createElement("a");
      link.href = window.URL.createObjectURL(blob);
      link.download = `${this.fileName}.${this.fileFormat}`;
      link.format = this.fileFormat;

      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
    },
    getMimeType(format) {
      const extension = format.toLowerCase();

      const mimeTypes = {
        pdf: "application/pdf",
        doc: "application/msword",
        docx: "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        xls: "application/vnd.ms-excel",
        xlsx: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        ppt: "application/vnd.ms-powerpoint",
        pptx: "application/vnd.openxmlformats-officedocument.presentationml.presentation",
        odt: "application/vnd.oasis.opendocument.text",
        ods: "application/vnd.oasis.opendocument.spreadsheet",
        jpg: "image/jpeg",
        jpeg: "image/jpeg",
        png: "image/png",
        gif: "image/gif",
        bmp: "image/bmp",
        svg: "image/svg+xml",
        webp: "image/webp",
        tiff: "image/tiff",
        ico: "image/x-icon",
        mp3: "audio/mpeg",
        wav: "audio/wav",
        ogg: "audio/ogg",
        midi: "audio/midi",
        flac: "audio/flac",
        aac: "audio/aac",
        mp4: "video/mp4",
        mkv: "video/x-matroska",
        webm: "video/webm",
        mpeg: "video/mpeg",
        avi: "video/x-msvideo",
        mov: "video/quicktime",
        wmv: "video/x-ms-wmv",
        zip: "application/zip",
        gzip: "application/gzip",
        "7z": "application/x-7z-compressed",
        tar: "application/x-tar",
        rar: "application/x-rar-compressed",
        txt: "text/plain",
        html: "text/html",
        css: "text/css",
        js: "application/javascript",
        json: "application/json",
        xml: "application/xml",
        csv: "text/csv",
        tsv: "text/tab-separated-values",
        rtf: "application/rtf",
        md: "text/markdown",
        exe: "application/x-msdownload",
        msi: "application/x-msi",
        bat: "application/x-msdos-program",
        sh: "application/x-sh",
        dll: "application/x-msdownload",
        eot: "application/vnd.ms-fontobject",
        ttf: "font/ttf",
        woff: "font/woff",
        woff2: "font/woff2",
        otf: "font/otf",
        epub: "application/epub+zip",
        azw: "application/vnd.amazon.ebook",
        mobi: "application/x-mobipocket-ebook",
        ics: "text/calendar",
        jar: "application/java-archive",
      };

      return mimeTypes[extension] || "application/octet-stream";
    },
  },
};
</script>

<style lang="scss">
.download_box {
  cursor: pointer;
  margin: 0;
  padding: 0;
}
</style>
