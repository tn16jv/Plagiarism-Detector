export class ComparisonResults {
    courseName: string;
    assignmentName: string;
    userReports: FileByLineModel[];
}


export class FileByLineModel {

    constructor(obj: FileByLineModel = null) {
        if (obj != null && obj != undefined) {
            this.userId = obj.userId;
            this.assignmentName = obj.assignmentName;
            this.copyFrom = obj.copyFrom;
        } else {
            this.copyFrom = [new CopyFromModel()];
        }
    }

    userId: number; // submitter of assignment
    assignmentName: string; // name of submitted file (i.e. A1-5772199-jp14fg)
    copyFrom: CopyFromModel[]; // who the user copied from & the lines they copied
}

export class CopyFromModel {

    constructor() {
        this.lines = new Map();
    }

    CopiedPercentage: number;
    lines: Map<number, CopyModel>; // 1-based index corresponding to a line number in the file
    copiedFromId: number; // ID of user being copied from
    assignName: string;
}

export class CopyModel {
    copied: boolean; // indicates whether or not the line is copied
    line: string; // the line of code
    copiedLineId: number; // the line number in the CopiedFrom file that the user copied
}